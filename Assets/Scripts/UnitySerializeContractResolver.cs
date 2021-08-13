using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

public class UnitySerializeContractResolver : DefaultContractResolver {
    public static UnitySerializeContractResolver Instance { get; } = new UnitySerializeContractResolver();

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
        JsonProperty property = base.CreateProperty(member, memberSerialization);
        switch (member.MemberType) {
            case MemberTypes.Method:
                property.Ignored = true;
                break;
            case MemberTypes.Event:
                property.Ignored = true;
                break;
            case MemberTypes.Constructor:
                property.Ignored = true;
                break;
        }
        return property;
    }
}
