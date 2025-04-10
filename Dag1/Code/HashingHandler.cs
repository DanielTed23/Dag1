using System.Security.Cryptography;
using System.Text;

namespace Dag1.Code;

public class HashingHandler
{
    public dynamic SHA256Hashing(dynamic valueToHash) =>
        valueToHash is byte[] bytes
            ? SHA256.Create().ComputeHash(bytes)
            : Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(valueToHash.ToString())));



    //MD5 hashing er ikke sikker!
    //public dynamic MD5Hashing(dynamic valueToHash) =>

    //    valueToHash is byte[] 
    //    ? MD5.Create().ComputeHash(valueToHash)
    //    : Convert.FromBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(valueToHash)));
}
