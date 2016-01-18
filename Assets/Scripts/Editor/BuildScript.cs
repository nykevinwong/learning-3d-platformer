using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

public class BuildScript : MonoBehaviour {

	public static string __build_output_folder = ".\\build\\";

	public static string __android_output_filename = "android\\android.apk";
	public static string __webgl_output_folder = "webgl";

	static string[] GetScenes()
	{
		return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
	}

	static void CompileAndroidBuild()
	{
		string final_path = __build_output_folder + __android_output_filename;
		BuildPipeline.BuildPlayer(GetScenes(), final_path, BuildTarget.Android, BuildOptions.None);
	}

	static void CompileWebGLBuild()
	{
		string final_path = __build_output_folder + __webgl_output_folder;
		BuildPipeline.BuildPlayer(GetScenes(), final_path, BuildTarget.WebGL, BuildOptions.None);
	}


}

