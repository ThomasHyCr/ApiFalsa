using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RandomUserResponse
{
    public List<RandomUserResult> results;
}

[System.Serializable]
public class RandomUserResult
{
    public NameData name;
    public PictureData picture;
    public string email;
}

[System.Serializable]
public class NameData
{
    public string title;
    public string first;
    public string last;
}

[System.Serializable]
public class PictureData
{
    public string large;
    public string medium;
    public string thumbnail;
}
