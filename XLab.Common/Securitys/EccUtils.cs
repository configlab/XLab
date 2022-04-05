using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace XLab.Common.Securitys
{
    /// <summary>
    /// 作者: http://config.net.cn
    /// 创建时间:2022-4-5
    /// </summary>
    public class EccUtils
    {
        public static ECDsa LoadPublicKey(byte[] keyBytes)
        {
            var pointBytes = keyBytes.TakeLast(64);
            var pubKeyX = pointBytes.Take(32).ToArray();
            var pubKeyY = pointBytes.TakeLast(32).ToArray();
            var ecdsa = ECDsa.Create(new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP256,
                Q = new ECPoint
                {
                    X = pubKeyX,
                    Y = pubKeyY
                }
            });
            return ecdsa;
        }
        public static ECDsa LoadPublicKey(string pubKey)
        {
            var keyBytes = Convert.FromBase64String(pubKey);
            return LoadPublicKey(keyBytes);
        }
    }
}
