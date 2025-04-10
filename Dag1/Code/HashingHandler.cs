using System.Security.Cryptography;
using System.Text;

namespace Dag1.Code;

public class HashingHandler
{
    public dynamic MD5Hashing(dynamic valueToHash) =>
    
        valueToHash is byte[] 
        ? MD5.Create().ComputeHash(valueToHash)
        : Convert.FromBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(valueToHash)));
    

   
}
