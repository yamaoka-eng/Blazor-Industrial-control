using System.Text.RegularExpressions;

namespace WHITE_20.PLC
{
    public class PLCRepository
    {
        
        public static string FormatOPCUAString(string input)
        {
            // 分割输入字符串，获取DB编号和其余部分
            var parts = input.Split(new char[] { '.' }, 2);
            if (parts.Length != 2)
            {
                throw new ArgumentException("Input string is not in the expected format.");
            }
            string dbNumber = parts[0];
            string remainingPath = parts[1];

            // 处理可能存在的数组索引
            var pathParts = remainingPath.Split('.');
            for (int i = 0; i < pathParts.Length; i++)
            {
                // 检查是否有数组索引，并相应地格式化
                var match = Regex.Match(pathParts[i], @"(\w+)\[(\d+)\]$");
                if (match.Success)
                {
                    // 如果有索引
                    pathParts[i] = $"\"{match.Groups[1].Value}\"[{match.Groups[2].Value}]";
                }
                else
                {
                    pathParts[i] = $"\"{pathParts[i]}\"";
                }
            }

            // 构造最终路径
            string fieldPath = string.Join(".", pathParts);

            // 返回包含DB编号的最终字符串，并用双引号包围DB编号
            return $"ns=3;s=\"{dbNumber}\".{fieldPath}";
        }
    }

    public static class DictionaryCreator
    {
        /// <summary>
        /// 创建一个字典，键的格式为 "keyPrefix[i]"，值的格式为 "prefix.keyPrefix[i]"。
        /// </summary>
        /// <param name="prefix">值中使用的前缀。</param>
        /// <param name="keyPrefix">字典键的前缀。</param>
        /// <param name="count">要创建的条目数量。</param>
        /// <returns>一个格式化了键和值的字典。</returns>
        public static Dictionary<string, object> CreateIndexedDictionary(
            string prefix,
            string keyPrefix,
            int count
        )
        {
            var dict = new Dictionary<string, object>();
            for (int i = 0; i < count; i++)
            {
                dict[$"{keyPrefix}[{i}]"] = $"{prefix}.{keyPrefix}[{i}]";
            }
            return dict;
        }

        /// <summary>
        /// 创建一个字典，键来自提供的名称，值的格式为 "prefix.name"。
        /// </summary>
        /// <param name="prefix">值中使用的前缀。</param>
        /// <param name="names">用作字典键的名称。</param>
        /// <returns>一个格式化了键和值的字典。</returns>
        public static Dictionary<string, object> CreateNamedDictionary(
            string prefix,
            params string[] names
        )
        {
            var dict = new Dictionary<string, object>();
            foreach (var name in names)
            {
                dict[name] = $"{prefix}.{name}";
            }
            return dict;
        }
    }
}
