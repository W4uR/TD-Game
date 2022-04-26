using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    #region consts

    static readonly string[] ADJECTIVES =
    {
        "Small","Giant","Sad","Heroic","Furious","Sick","Basic","Sweet","Weak","Strong","Old","Young","Sus","Caring","Selfish","Lonely","Friendly","Clever","Idiot","Careful",
        "Crazy","Wayward","Charming","Boring","Cunning","Grumpy","Magical"
    };


    //https://www.imagineforest.com/blog/fantasy-character-names/
    static readonly string[] NAMES =
    {
        "Amara","Darvyn","Slyvek","Kyra","Soldelle","Sakakara","Phyrra","Jinvia","Nalra","Edmyla","Delvin","Starburst","Sunshine","Fada","Mistmael","Azura","Laimus","Silver Star",
        "Thingol","Ailmar","Jewel","Voggur","Devella","Breya","Quamara","Taena","Lunarex","Vesryn","Bonna","Narbeth"
    };

    #endregion


    public static string GenerateRandomName()
    {

        return NAMES[Random.Range(0, NAMES.Length)] + " the " + ADJECTIVES[Random.Range(0, ADJECTIVES.Length)];
    }
}
