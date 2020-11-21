using MelonLoader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GTFO.Custom.Rundown.Converters
{
	public class StringUintDelegateConverter : JsonConverter
	{
		private readonly Func<string, bool> _IsSupportedFunc;
		private readonly Func<string, uint> _ReturnFunc;

		private StringUintDelegateConverter() { }

		public StringUintDelegateConverter(Func<string, bool> isSupportedFunc, Func<string, uint> returnFunc)
        {
			_IsSupportedFunc = isSupportedFunc;
			_ReturnFunc = returnFunc;
        }

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(uint);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}

			JToken token = JToken.Load(reader);
			if (_IsSupportedFunc != null && _IsSupportedFunc.Invoke(reader.Path))
			{
				if (reader.TokenType == JsonToken.String)
				{
					if (_ReturnFunc != null)
					{
						return _ReturnFunc.Invoke(token.ToObject<string>());
					}
				}
				else if (reader.TokenType == JsonToken.Integer) //integer is just integer
				{
					MelonLogger.LogWarning("This Rundown has used const persistentID on their file! It may can cause crash with other mods!");
					MelonLogger.LogWarning($" - Location: {reader.Path}");
					var id = token.ToObject<uint>();
					return id;
				}
			}
			else if (reader.TokenType == JsonToken.Integer)
			{
				var id = token.ToObject<uint>();
				return id;
			}
			else if (reader.TokenType == JsonToken.String)
			{
				MelonLogger.LogError("This Rundown has used string to unsupported items!");
				MelonLogger.LogError($" - Location: {reader.Path}");
				throw new InvalidOperationException();
			}

				

			return null;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			return;
		}

		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

        public override bool CanWrite
        {
			get
            {
				return false;
            }
        }
    }
}
