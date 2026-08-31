using System;
using UnityEngine;

/// <summary>
/// Procedurally generates a starfield as a single mesh (one quad per star),
/// rendered with an unlit additive shader. Designed for URP.
///
/// Attach to an empty GameObject with a MeshFilter + MeshRenderer.
/// Assign the "Starfield" shader (or a material using it) to the renderer,
/// or let this script create the material at runtime.
/// </summary>
[ExecuteAlways]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class StarfieldGenerator : MonoBehaviour
{
    public enum DistributionMode
    {
        SphereShell,   // stars scattered over a sphere shell (good for a 3D skybox-style backdrop)
        FlatDisc       // stars scattered on a flat disc (good for a 2D parallax background plane)
    }

    [Header("Generation")]
    [Tooltip("Same seed always produces the same starfield layout.")]
    public int seed = 12345;
    [Range(10, 20000)] public int starCount = 3000;
    public DistributionMode distribution = DistributionMode.FlatDisc;

    [Header("Placement")]
    [Tooltip("For SphereShell: sphere radius. For FlatDisc: disc radius.")]
    public float radius = 50f;
    [Tooltip("For FlatDisc only: random depth jitter along the plane normal, for subtle parallax layers.")]
    public float depthJitter = 5f;

    [Header("Size")]
    public float minStarSize = 0.05f;
    public float maxStarSize = 0.35f;
    [Tooltip("Higher = fewer big stars, more small ones (mimics real star luminosity distribution).")]
    [Range(0.5f, 5f)] public float sizeBiasPower = 2.5f;

    [Header("Color (blackbody temperature in Kelvin)")]
    [Tooltip("Cool/red end of the range, e.g. 3000K.")]
    public float minTemperatureK = 3000f;
    [Tooltip("Hot/blue end of the range, e.g. 15000K.")]
    public float maxTemperatureK = 15000f;
    [Range(0f, 1f)] public float colorSaturation = 0.85f;

    [Header("Brightness / Twinkle")]
    [Range(0f, 3f)] public float brightnessMultiplier = 1.2f;
    [Tooltip("Stored per-star as vertex color alpha; sampled by the shader for twinkle animation.")]
    public bool enableTwinkleData = true;

    [Header("Clustering (optional)")]
    [Tooltip("Use Perlin noise to bias density, so stars form soft clusters instead of pure uniform scatter.")]
    public bool useClusterNoise = false;
    public float clusterNoiseScale = 0.05f;
    [Range(0f, 1f)] public float clusterStrength = 0.5f;

    Mesh _mesh;
    MeshFilter _mf;
    MeshRenderer _mr;

    void OnEnable()
    {
        _mf = GetComponent<MeshFilter>();
        _mr = GetComponent<MeshRenderer>();
        Generate();
    }

    [ContextMenu("Regenerate")]
    public void Generate()
    {
        if (_mf == null) _mf = GetComponent<MeshFilter>();
        if (_mr == null) _mr = GetComponent<MeshRenderer>();

        var rng = new System.Random(seed);

        // 4 verts + 6 indices per star (a quad, expanded to face camera in the shader)
        var vertices = new Vector3[starCount * 4];
        var uvs = new Vector2[starCount * 4];       // xy = quad corner (-1..1), used for billboarding + falloff
        var uv2 = new Vector2[starCount * 4];        // x = star size, y = twinkle phase seed
        var colors = new Color[starCount * 4];
        var triangles = new int[starCount * 6];

        int placed = 0;
        int attempts = 0;
        int maxAttempts = starCount * 8;

        while (placed < starCount && attempts < maxAttempts)
        {
            attempts++;

            Vector3 pos = distribution == DistributionMode.SphereShell
                ? RandomOnSphere(rng) * radius
                : RandomInDisc(rng, rng);

            // Optional clustering: reject some stars in low-density noise regions
            if (useClusterNoise)
            {
                float n = Mathf.PerlinNoise(
                    (pos.x) * clusterNoiseScale + seed * 0.001f,
                    (distribution == DistributionMode.SphereShell ? pos.y : pos.z) * clusterNoiseScale);
                float density = Mathf.Lerp(1f, n, clusterStrength);
                if ((float)rng.NextDouble() > density) continue;
            }

            // Size: bias toward small stars (power curve), a handful of big bright ones
            float sizeT = Mathf.Pow((float)rng.NextDouble(), sizeBiasPower);
            float size = Mathf.Lerp(minStarSize, maxStarSize, 1f - sizeT);

            // Color from blackbody temperature
            float tempT = (float)rng.NextDouble();
            float kelvin = Mathf.Lerp(minTemperatureK, maxTemperatureK, tempT);
            Color starColor = KelvinToRGB(kelvin);
            starColor = Color.Lerp(Color.white, starColor, colorSaturation);

            // Brighter stars tend to be the bigger ones
            float brightness = Mathf.Lerp(0.5f, 1.5f, 1f - sizeT) * brightnessMultiplier;
            starColor *= brightness;
            starColor.a = enableTwinkleData ? (float)rng.NextDouble() : 1f; // twinkle phase seed

            float twinkleSeed = (float)rng.NextDouble() * 100f;

            int vBase = placed * 4;
            Vector2[] corners = { new(-1, -1), new(1, -1), new(1, 1), new(-1, 1) };
            for (int c = 0; c < 4; c++)
            {
                vertices[vBase + c] = pos;
                uvs[vBase + c] = corners[c];
                uv2[vBase + c] = new Vector2(size, twinkleSeed);
                colors[vBase + c] = starColor;
            }

            int tBase = placed * 6;
            triangles[tBase + 0] = vBase + 0;
            triangles[tBase + 1] = vBase + 2;
            triangles[tBase + 2] = vBase + 1;
            triangles[tBase + 3] = vBase + 0;
            triangles[tBase + 4] = vBase + 3;
            triangles[tBase + 5] = vBase + 2;

            placed++;
        }

        // Trim arrays if we placed fewer than requested (can happen with aggressive clustering rejection)
        if (placed != starCount)
        {
            Array.Resize(ref vertices, placed * 4);
            Array.Resize(ref uvs, placed * 4);
            Array.Resize(ref uv2, placed * 4);
            Array.Resize(ref colors, placed * 4);
            Array.Resize(ref triangles, placed * 6);
        }

        if (_mesh == null)
        {
            _mesh = new Mesh { name = "ProceduralStarfield" };
            _mesh.indexFormat = starCount * 4 > 65000
                ? UnityEngine.Rendering.IndexFormat.UInt32
                : UnityEngine.Rendering.IndexFormat.UInt16;
        }
        else
        {
            _mesh.Clear();
        }

        _mesh.vertices = vertices;
        _mesh.uv = uvs;
        _mesh.uv2 = uv2;
        _mesh.colors = colors;
        _mesh.triangles = triangles;
        _mesh.RecalculateBounds();
        // Bloat bounds since verts sit at star centers but the shader expands them outward as billboards
        var b = _mesh.bounds;
        b.Expand(maxStarSize * 2f);
        _mesh.bounds = b;

        _mf.sharedMesh = _mesh;

        if (_mr.sharedMaterial == null)
        {
            var shader = Shader.Find("Custom/Starfield");
            if (shader != null)
                _mr.sharedMaterial = new Material(shader) { name = "Starfield (Generated)" };
        }
    }

    Vector3 RandomOnSphere(System.Random rng)
    {
        // Uniform distribution on a sphere
        double u = rng.NextDouble();
        double v = rng.NextDouble();
        double theta = 2.0 * Math.PI * u;
        double phi = Math.Acos(2.0 * v - 1.0);
        float x = (float)(Math.Sin(phi) * Math.Cos(theta));
        float y = (float)(Math.Sin(phi) * Math.Sin(theta));
        float z = (float)Math.Cos(phi);
        return new Vector3(x, y, z);
    }

    Vector3 RandomInDisc(System.Random rngA, System.Random rngB)
    {
        // Uniform distribution across a disc area (sqrt trick avoids center clustering)
        double r = radius * Math.Sqrt(rngA.NextDouble());
        double theta = 2.0 * Math.PI * rngB.NextDouble();
        float x = (float)(r * Math.Cos(theta));
        float y = (float)(r * Math.Sin(theta));
        float z = ((float)rngA.NextDouble() - 0.5f) * depthJitter;
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Approximates blackbody radiation color for a given temperature in Kelvin.
    /// Based on Tanner Helland's widely-used approximation algorithm.
    /// </summary>
    public static Color KelvinToRGB(float kelvin)
    {
        float temp = kelvin / 100f;
        float r, g, b;

        if (temp <= 66f)
        {
            r = 255f;
            g = 99.4708025861f * Mathf.Log(temp) - 161.1195681661f;
        }
        else
        {
            r = 329.698727446f * Mathf.Pow(temp - 60f, -0.1332047592f);
            g = 288.1221695283f * Mathf.Pow(temp - 60f, -0.0755148492f);
        }

        if (temp >= 66f) b = 255f;
        else if (temp <= 19f) b = 0f;
        else b = 138.5177312231f * Mathf.Log(temp - 10f) - 305.0447927307f;

        r = Mathf.Clamp(r, 0f, 255f);
        g = Mathf.Clamp(g, 0f, 255f);
        b = Mathf.Clamp(b, 0f, 255f);

        return new Color(r / 255f, g / 255f, b / 255f, 1f);
    }
}
