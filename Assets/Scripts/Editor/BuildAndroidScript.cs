using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

public class BuildAndroidScript : MonoBehaviour {

	public static string __build_output_folder = ".\\Build\\";
	public static string __android_output_filename = "android.apk";

		static void StartBuild()
		{
			string final_apk_path = __build_output_folder + __android_output_filename;
			BuildPipeline.BuildPlayer(GetScenes(), final_apk_path, BuildTarget.Android, BuildOptions.None);
		}
		
		static string[] GetScenes()
		{
			return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
		}
}
