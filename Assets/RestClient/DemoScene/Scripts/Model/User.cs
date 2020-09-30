using System;


[Serializable]
public class User
{
	public string username;

    public Record record;

    public string localid;

	public override string ToString(){
		return UnityEngine.JsonUtility.ToJson (this, true);
	}

    public User(string name,  string localid)
    {
        this.username = name;
        this.localid = localid;
    }
}
