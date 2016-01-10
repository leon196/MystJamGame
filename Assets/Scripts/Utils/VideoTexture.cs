using UnityEngine;
using System.Collections;

public class VideoTexture : MonoBehaviour
{
	string videoName = "water.ogv";
	Material videoMaterial;

	void Start ()
	{
		videoMaterial = GetComponent<Renderer>().material;
		StartCoroutine(LoadMovieTexture());
	} 

	IEnumerator LoadMovieTexture ()
	{
		string URL = "file://" + Application.dataPath + "/StreamingAssets/" + videoName;

		WWW www = new WWW(URL);

		MovieTexture movieTexture = www.movie as MovieTexture;
		movieTexture.loop = true;

		while (!movieTexture.isReadyToPlay) {
			yield return 0;
		}

		videoMaterial.mainTexture = movieTexture;
		movieTexture.Play();
	}
}
