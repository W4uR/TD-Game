using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    #region consts
    
    static readonly string[] ADJECTIVES =
    {
        "Small","Giant","Sad","Heroic","Furious","Sick","Basic","Sweet","Weak","Strong","Old","Young","Sus","Caring","Selfish","Lonely","Friendly","Clever","Idiot","Careful",
        "Crazy","Wayward","Charming","Boring","Cunning","Grumpy","Magical","Algebra"
    };


    //https://www.imagineforest.com/blog/fantasy-character-names/
    static readonly string[] NAMES =
    {
        "Amara","Darvyn","Slyvek","Kyra","Soldelle","Sakakara","Phyrra","Jinvia","Nalra","Edmyla","Delvin","Starburst","Sunshine","Fada","Mistmael","Azura","Laimus","Silver Star",
        "Thingol","Ailmar","Jewel","Voggur","Devella","Breya","Quamara","Taena","Lunarex","Vesryn","Bonna","Narbeth","Linah","Burito"
    };

    #endregion

    public static Vector3 GRAVITY => Vector3.down * 9.16f;

    public static string GenerateRandomName()
    {

        return NAMES[Random.Range(0, NAMES.Length)] + " the " + ADJECTIVES[Random.Range(0, ADJECTIVES.Length)];
    }

    public static void MurderChildNoWitnesses(GameObject obj)
    {
        obj.transform.SetParent(null);
        GameObject.Destroy(obj);
    }
    public static Dictionary<byte, string> EnemyName = new Dictionary<byte, string>()
    {
        {0,"Goblin" },{1,"Skeleton"},{2,"Orc"}
    };
    public static Dictionary<string, byte> EnemyId = new Dictionary<string, byte>()
    {
        {"Goblin",0 },{"Skeleton",1},{"Orc",2}
    };
}
