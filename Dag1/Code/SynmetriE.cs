using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;

namespace Dag1.Code;

public class SynmetriE
{
    private readonly IDataProtector _key;

    public SynmetriE(IDataProtectionProvider key)
    {
        _key = key.CreateProtector("");
        _key = key.CreateProtector(new RSACryptoServiceProvider().ToXmlString(false));
    }

    public string Encrypt(string valueToEncrypt) =>
    
        _key.Protect(valueToEncrypt);

    public string Decrypt(string valueToDecrypt) =>

        _key.Unprotect(valueToDecrypt);

}
