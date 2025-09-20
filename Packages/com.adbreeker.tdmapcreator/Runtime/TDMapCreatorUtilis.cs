#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace adbreeker.TDMapCreator
{
    public class TDMapCreatorUtilis : MonoBehaviour
    {
        public static void LaunchTemporalScene(string scenePath)
        {
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

            if (sceneAsset == null)
            {
                EditorUtility.DisplayDialog("Map Creator", $"Cannot find scene at:\n{scenePath}", "OK");
                return;
            }

            EditorSceneManager.playModeStartScene = sceneAsset;
            EditorApplication.isPlaying = true;
        }

        public static Sprite[] GetSpritesFromTextureAsset(Object textureAsset)
        {
            if (textureAsset == null) return new Sprite[0];
            string path = AssetDatabase.GetAssetPath(textureAsset);
            if (string.IsNullOrEmpty(path)) return new Sprite[0];

            // For Multiple sprites, this returns each Sprite sub-asset; for Single, it returns one.
            return AssetDatabase.LoadAllAssetRepresentationsAtPath(path)
                .OfType<Sprite>()
                .DefaultIfEmpty(AssetDatabase.LoadAssetAtPath<Sprite>(path))
                .Where(s => s != null)
                .ToArray();
        }

        public static Texture2D CaptureMapAlignedToBackground(
            Transform mapRoot,
            SpriteRenderer background,
            int resWidth,
            int resHeight,
            int msaa = 4)
        {
            if (mapRoot == null || background == null || background.sprite == null)
                return null;

            // Exact target resolution from manager
            int outW = resWidth;
            int outH = resHeight;

            // World-space bounds of the background (includes its scale)
            var bgBounds = background.bounds;
            float worldW = bgBounds.size.x;
            float worldH = bgBounds.size.y;
            if (worldW <= 0f || worldH <= 0f) return null;

            // Temporary camera
            var camGO = new GameObject("MapCaptureCamera") { hideFlags = HideFlags.HideAndDontSave };
            var cam = camGO.AddComponent<Camera>();
            cam.enabled = false;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.orthographic = true;
            cam.allowHDR = false;
            cam.allowMSAA = msaa > 1;

            // Position and frame exactly the background
            cam.transform.position = new Vector3(bgBounds.center.x, bgBounds.center.y, bgBounds.center.z - 10f);
            cam.transform.rotation = Quaternion.identity;
            cam.nearClipPlane = 0.01f;
            cam.farClipPlane = 1000f;

            // Ensure camera frustum uses the requested aspect and covers the background fully.
            cam.aspect = (float)outW / outH;
            // Coverage: height = 2*size, width = 2*size*aspect. Background is already scaled to match res aspect.
            cam.orthographicSize = worldH * 0.5f;

            // Prepare RT with exact requested resolution
            var rt = new RenderTexture(outW, outH, 24, RenderTextureFormat.ARGB32)
            {
                antiAliasing = Mathf.ClosestPowerOfTwo(Mathf.Clamp(msaa, 1, 8)),
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };

            // Disable everything not under mapRoot (so only Map renders)
            var disabledRenderers = new List<Renderer>();
            foreach (var r in Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None))
            {
                if (!r || !r.enabled) continue;
                if (!r.transform.IsChildOf(mapRoot))
                {
                    r.enabled = false;
                    disabledRenderers.Add(r);
                }
            }
            var disabledCanvases = new List<Canvas>();
            foreach (var c in Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None))
            {
                if (!c || !c.enabled) continue;
                if (!c.transform.IsChildOf(mapRoot))
                {
                    c.enabled = false;
                    disabledCanvases.Add(c);
                }
            }

            Texture2D tex = null;
            var prevActive = RenderTexture.active;
            try
            {
                cam.targetTexture = rt;
                RenderTexture.active = rt;
                cam.Render();

                tex = new Texture2D(outW, outH, TextureFormat.RGB24, false, false); // opaque
                tex.ReadPixels(new Rect(0, 0, outW, outH), 0, 0);
                tex.Apply(false, false);
            }
            finally
            {
                foreach (var r in disabledRenderers) if (r) r.enabled = true;
                foreach (var c in disabledCanvases) if (c) c.enabled = true;

                cam.targetTexture = null;
                RenderTexture.active = prevActive;

                if (Application.isPlaying)
                {
                    Object.Destroy(rt);
                    Object.Destroy(camGO);
                }
                else
                {
                    Object.DestroyImmediate(rt);
                    Object.DestroyImmediate(camGO);
                }
            }

            return tex;
        }
    }
}
#endif