using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using tutorial;

public class Proto2 : MonoBehaviour
{
    void Start()
    {
        Person person1 = new Person();
        person1.name = "acc";
        person1.id = 5;
        person1.email = "33";

        //序列化
        MemoryStream ms = new MemoryStream();
        ProtoBuf.Serializer.Serialize<Person>(ms, person1);
        byte[] newbyte = ms.ToArray();
        Debug.Log(newbyte.Length);

        //反序列化
        MemoryStream newms = new MemoryStream(newbyte);
        Person person2 = ProtoBuf.Serializer.Deserialize<Person>(newms);
        Debug.Log(person2.name + ", " + person2.email);
    }
}
