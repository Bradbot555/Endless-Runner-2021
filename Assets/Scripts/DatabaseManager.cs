using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class DatabaseManager : MonoBehaviour
{
    public delegate void FetchObjectsDataAction(List<ObjectData> objectData);
    public static event FetchObjectsDataAction onFetchedObjectData;

    [SerializeField] string baseURL = "";
    public static DatabaseManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
    }

    void Start()
    {
        FetchObject();
    }

    public void FetchObject()
    {
        StartCoroutine(FetchObjectAPICall());
    }

    public void AddObject(ObjectData data)
    {
        StartCoroutine(AddObjectAPICall(data));
    }

    IEnumerator FetchObjectAPICall()
    {
        UnityWebRequest www = UnityWebRequest.Get(baseURL + "ObjectData.json");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Object(s) fetched from database");
            ParseFetchedObjects(www.downloadHandler.text);
        }

    }

    IEnumerator AddObjectAPICall(ObjectData data)
    {
        UnityWebRequest www = new UnityWebRequest(baseURL + "ObjectData.json", "POST");

        //Convert data to Json string and then to byte array
        byte[] body = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));

        www.uploadHandler = new UploadHandlerRaw(body);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Object added to database");
        }
    }

    void ParseFetchedObjects(string jsonString)
    {
        JSONNode rootNode = JSON.Parse(jsonString);
        List<ObjectData> objects = new List<ObjectData>();

        foreach(JSONNode obj in rootNode)
        {
            string type = obj["type"];
            float x = obj["x"].AsFloat;
            float y = obj["y"].AsFloat;
            float z = obj["z"].AsFloat;

            ObjectData data = new ObjectData(type, x, y, z);
            objects.Add(data);
        }

        if(onFetchedObjectData != null)
        {
            onFetchedObjectData(objects);
        }
    }
}
