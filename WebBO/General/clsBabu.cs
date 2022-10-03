
using System;
using System.Net;
using System.Windows;

using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace WebBO.General
{
	/// <summary>
	/// 處理動態相關的ㄅㄚㄅㄨ工具類別
	/// </summary>
	public class TBabuTools
	{
		public TBabuTools()
		{
		}

		/// <summary>
		/// 把 Dictionary 的值填入任意類別的屬性裡
		/// Class 必須為 public，只有 Class.Property 才會被填寫
		/// </summary>
		/// <param name="obj">要填入的類別實例</param>
		/// <param name="aPropertyValue">Dictionary 格式的屬性-值</param>
		static public void FillProperty(object obj,Dictionary<string, string> aPropertyValue)
		{
			Type clsType = obj.GetType();

			foreach (KeyValuePair<string, string> kvp in aPropertyValue)
			{
				PropertyInfo pi = clsType.GetProperty(kvp.Key);
				if (pi == null || !pi.CanWrite) continue;

				object v = kvp.Value;

				if (v != null)
				{
					switch (pi.PropertyType.ToString())
					{
						case "System.String":
							v = kvp.Value;
							break;

						case "System.Int16":
							v = Int16.Parse(kvp.Value);
							break;

						case "System.Int32":
							v = Int32.Parse(kvp.Value);
							break;

						case "System.Int64":
							v = Int64.Parse(kvp.Value);
							break;

						case "System.Double":
							v = Double.Parse(kvp.Value);
							break;

						case "System.DateTime":
							v = DateTime.Parse(kvp.Value);
							break;                     
					}

					pi.SetValue(obj, v, null);
				}
			}
		}

		/// <summary>
		/// 產生一個任意類別的實例，並把 Dictionary 裡面的值填到屬性裡
		/// 範例：
		/// Dictionary&lt;string,string&gt; dic = new Dictionary&lt;string,string&gt; ();
		/// dic["p1"] = "v1";
		/// dic["p2"] = "v2";
		/// TClass cls = CreateInstance&lt;TClass&gt;(dic);
		/// </summary>
		/// <typeparam name="T">Class Type</typeparam>
		/// <param name="aPropertyValue">Dictionary 格式的屬性-值</param>
		/// <returns></returns>
		static public T CreateInstance<T>(Dictionary<string, string> aPropertyValue)
		{
			T ClassIns = Activator.CreateInstance<T>();
			FillProperty(ClassIns, aPropertyValue);
			return ClassIns;
		}

		// 這是測試用的Method
		//public static Type CreateTypeTest(string aClassName)
		//{
		//  try
		//  {
		//    AssemblyName an = new AssemblyName();
		//    an.Name = Assembly.GetCallingAssembly().FullName;
		//    AppDomain ad = System.Threading.Thread.GetDomain();
		//    AssemblyBuilder ab = ad.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
		//    ModuleBuilder mb = ab.DefineDynamicModule(an.FullName);
		//    TypeBuilder tb = mb.DefineType(aClassName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass);

		//    //第一個參數是屬性名稱，第二個參數是屬性的標籤，第三個參數是傳回的參數型別，第四個參數是傳入的參數型別。
		//    PropertyBuilder pb = tb.DefineProperty(
		//      "Number",
		//      PropertyAttributes.HasDefault,
		//      typeof(string),
		//      null);

		//    //第二個參數是欄位名稱，第二個參數是欄位的型別，第三個參數是欄位的標籤，其中標識為私有（Private）欄位。
		//    FieldBuilder fb = tb.DefineField(
		//        "_Number",
		//        typeof(string),
		//        FieldAttributes.Private);


		//    //定義存取方法的屬性，其中要做為類別屬性的存取方法，必需要有特別的屬性，亦即要有MethodAttributes.SpecialName及MethodAttributes.HideSig。http://dev.ischool.com.tw/wiki/index.php/User:ChangKH
		//    MethodAttributes maGetSet = MethodAttributes.Public |
		//       MethodAttributes.SpecialName | MethodAttributes.HideBySig;
 
		//    //定義取得方法，第一個參數是方法名稱，第二個參數是描述方法的屬性，第三個參數是傳回的型別，第四個參數是傳入的型別。
		//    MethodBuilder mbGetAccessor = tb.DefineMethod(
		//       "get_Number",
		//       maGetSet,
		//       typeof(string),
		//       Type.EmptyTypes);
 
		//    //定義取得方法的中介語言，第二行是代表將我們剛才定義的FieldBuilder欄位值傳回。
		//    ILGenerator numberGetIL = mbGetAccessor.GetILGenerator();
 
		//    numberGetIL.Emit(OpCodes.Ldarg_0);
		//    numberGetIL.Emit(OpCodes.Ldfld, fb);
		//    numberGetIL.Emit(OpCodes.Ret);
 
		//    //定義設定方法，其中第一個參數是方法名稱，第二個參數是描述方法的屬性，第三個參數是傳入的參數型別，一般都是在屬性的設定都是藉由value參數來設定，所以此處傳入null，最後一個參數則是傳回的型別。
		//    MethodBuilder mbSetAccessor = tb.DefineMethod(
		//       "set_Number",
		//       maGetSet,
		//       null,
		//       new Type[] { typeof(string)});
 
		//    ILGenerator numberSetIL = mbSetAccessor.GetILGenerator();
 
		//    //定義設定方法的中介語言，第三行是將傳入值設定到我們定義的FieldBuilder欄位。
		//    numberSetIL.Emit(OpCodes.Ldarg_0);
		//    numberSetIL.Emit(OpCodes.Ldarg_1);
		//    numberSetIL.Emit(OpCodes.Stfld, fb);
		//    numberSetIL.Emit(OpCodes.Ret);
 
		//    //在定義好存取及設定方法後，接下來將方法加入到屬性當中。
		//    pb.SetGetMethod(mbGetAccessor);
		//    pb.SetSetMethod(mbSetAccessor);

		//    return tb.CreateType();
		//  }
		//  catch (Exception ex)
		//  {
		//    Console.WriteLine("Exception: {0}", ex.Message);
		//    return null;
		//  }
		//}

		public static Type CreateType(string aClassName, string [] aProperties)
		{
			try
			{
				AssemblyName an = new AssemblyName();
				an.Name = Assembly.GetCallingAssembly().FullName;
				AppDomain ad = System.Threading.Thread.GetDomain();
				AssemblyBuilder ab = ad.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
				ModuleBuilder mb = ab.DefineDynamicModule(an.FullName);
				TypeBuilder tb = mb.DefineType(aClassName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass, typeof(TBabuClass));

				//foreach (KeyValuePair<string, string> kvp in aPropertyValue)
				foreach(string key in aProperties)
				{
					//第一個參數是屬性名稱，第二個參數是屬性的標籤，第三個參數是傳回的參數型別，第四個參數是傳入的參數型別。
					PropertyBuilder pb = 
						tb.DefineProperty(
								key,
								PropertyAttributes.HasDefault,
								typeof(string),
								null);

					//第一個參數是欄位名稱，第二個參數是欄位的型別，第三個參數是欄位的標籤，其中標識為私有（Private）欄位。
					FieldBuilder fb = 
						tb.DefineField(
								"_" + key,
								typeof(string),
								FieldAttributes.Private);

					//定義存取方法的屬性，其中要做為類別屬性的存取方法，必需要有特別的屬性，亦即要有MethodAttributes.SpecialName及MethodAttributes.HideSig。http://dev.ischool.com.tw/wiki/index.php/User:ChangKH
					MethodAttributes maGetSet = MethodAttributes.Public |
																			MethodAttributes.SpecialName | 
																			MethodAttributes.HideBySig;

					//定義取得方法，第一個參數是方法名稱，第二個參數是描述方法的屬性，第三個參數是傳回的型別，第四個參數是傳入的型別。
					MethodBuilder mbGetAccessor = 
						tb.DefineMethod(
							 "get_" + key,
							 maGetSet,
							 typeof(string),
							 Type.EmptyTypes);

					//定義取得方法的中介語言，第二行是代表將我們剛才定義的FieldBuilder欄位值傳回。
					ILGenerator ilgGet = mbGetAccessor.GetILGenerator();
					ilgGet.Emit(OpCodes.Ldarg_0);
					ilgGet.Emit(OpCodes.Ldfld, fb);
					ilgGet.Emit(OpCodes.Ret);


					//定義設定方法，其中第一個參數是方法名稱，第二個參數是描述方法的屬性，第三個參數是傳入的參數型別，一般都是在屬性的設定都是藉由value參數來設定，所以此處傳入null，最後一個參數則是傳回的型別。
					MethodBuilder mbSetAccessor = 
						tb.DefineMethod(
							 "set_" + key,
							 maGetSet,
							 null,
							 new Type[] { typeof(string) });
				
					ILGenerator ilgSet = mbSetAccessor.GetILGenerator();
					//定義設定方法的中介語言，第三行是將傳入值設定到我們定義的FieldBuilder欄位。
					ilgSet.Emit(OpCodes.Ldarg_0);
					ilgSet.Emit(OpCodes.Ldarg_1);
					ilgSet.Emit(OpCodes.Stfld, fb);
					ilgSet.Emit(OpCodes.Ret);

					//在定義好存取及設定方法後，接下來將方法加入到屬性當中。
					pb.SetGetMethod(mbGetAccessor);
					pb.SetSetMethod(mbSetAccessor);
				}

				return tb.CreateType();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: {0}", ex.Message);
				return null;
			}
		}

		public static Type CreateTypeFrom(Type aCopyFrom, string[] aProperties, Type[] aPropertiesType)
		{
			try
			{
				AssemblyName an = new AssemblyName();
				an.Name = Assembly.GetCallingAssembly().FullName;
				AppDomain ad = System.Threading.Thread.GetDomain();
				AssemblyBuilder ab = ad.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
				ModuleBuilder mb = ab.DefineDynamicModule(an.FullName);
				TypeBuilder tb = mb.DefineType(aCopyFrom.Name + "_Copy", TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass, aCopyFrom);

				//foreach (KeyValuePair<string, string> kvp in aPropertyValue)
				int idx = 0;
				foreach (string key in aProperties)
				{
					Type NewType = typeof(object);
					
					if (idx < aPropertiesType.Length)
						NewType = aPropertiesType[idx];

					idx++;

					//第一個參數是屬性名稱，第二個參數是屬性的標籤，第三個參數是傳回的參數型別，第四個參數是傳入的參數型別。
					PropertyBuilder pb =
						tb.DefineProperty(
								key,
								PropertyAttributes.HasDefault,
								NewType,
								null);

					//第一個參數是欄位名稱，第二個參數是欄位的型別，第三個參數是欄位的標籤，其中標識為私有（Private）欄位。
					FieldBuilder fb =
						tb.DefineField(
								"_" + key,
								NewType,
								FieldAttributes.Private);

					//定義存取方法的屬性，其中要做為類別屬性的存取方法，必需要有特別的屬性，亦即要有MethodAttributes.SpecialName及MethodAttributes.HideSig。http://dev.ischool.com.tw/wiki/index.php/User:ChangKH
					MethodAttributes maGetSet = MethodAttributes.Public |
																			MethodAttributes.SpecialName |
																			MethodAttributes.HideBySig;

					//定義取得方法，第一個參數是方法名稱，第二個參數是描述方法的屬性，第三個參數是傳回的型別，第四個參數是傳入的型別。
					MethodBuilder mbGetAccessor =
						tb.DefineMethod(
							 "get_" + key,
							 maGetSet,
							 NewType,
							 Type.EmptyTypes);

					//定義取得方法的中介語言，第二行是代表將我們剛才定義的FieldBuilder欄位值傳回。
					ILGenerator ilgGet = mbGetAccessor.GetILGenerator();
					ilgGet.Emit(OpCodes.Ldarg_0);
					ilgGet.Emit(OpCodes.Ldfld, fb);
					ilgGet.Emit(OpCodes.Ret);


					//定義設定方法，其中第一個參數是方法名稱，第二個參數是描述方法的屬性，第三個參數是傳入的參數型別，一般都是在屬性的設定都是藉由value參數來設定，所以此處傳入null，最後一個參數則是傳回的型別。
					MethodBuilder mbSetAccessor =
						tb.DefineMethod(
							 "set_" + key,
							 maGetSet,
							 null,
							 new Type[] { NewType });

					ILGenerator ilgSet = mbSetAccessor.GetILGenerator();
					//定義設定方法的中介語言，第三行是將傳入值設定到我們定義的FieldBuilder欄位。
					ilgSet.Emit(OpCodes.Ldarg_0);
					ilgSet.Emit(OpCodes.Ldarg_1);
					ilgSet.Emit(OpCodes.Stfld, fb);
					ilgSet.Emit(OpCodes.Ret);

					//在定義好存取及設定方法後，接下來將方法加入到屬性當中。
					pb.SetGetMethod(mbGetAccessor);
					pb.SetSetMethod(mbSetAccessor);
				}

				return tb.CreateType();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: {0}", ex.Message);
				return null;
			}
		}

		public static Type CreateType(string aClassName, Dictionary<string, string> aPropertyValue)
		{
			string[] prop = new string[aPropertyValue.Count];
			aPropertyValue.Keys.CopyTo(prop, 0);
			return CreateType(aClassName,prop);
		}
	}

	/// <summary>
	/// 動態類別的基礎類別
	/// </summary>
    public class TBabuClass: INotifyPropertyChanged
    {

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private Type ClassType = null;

		public string this[string aPropertyName]
		{
			#region 取得屬性的值
			get
			{
				PropertyInfo pi = this.GetType().GetProperty(aPropertyName);
				if (pi != null)
					return (string)pi.GetValue(this, null);
				else
					return null;
			}
			set
			{
				PropertyInfo pi = this.GetType().GetProperty(aPropertyName);
                if (pi != null)
                {
                    pi.SetValue(this, value, null);
                    OnPropertyChanged(aPropertyName);
                }
			} 
			#endregion
		}

		/// <summary>
		/// 將 Dictionary 的值拿來產生新 Type 並把 Key 當屬性
		/// 第一次呼叫時會自動產生新 Type，往後的呼叫都使用此 Type 產生實例
		/// </summary>
		/// <param name="aPropertyValue"></param>
		/// <returns></returns>
		public TBabuClass CreateInstance(Dictionary<string, string> aPropertyValue)
		{
			if (ClassType == null)
				ClassType = TBabuClass.CreateType(aPropertyValue);

			if (ClassType == null) return null;

			object bc = Activator.CreateInstance(ClassType);
			TBabuTools.FillProperty(bc, aPropertyValue);
			return (TBabuClass)bc;
		}

		public TBabuClass CreateInstance(string [] aProperties)
		{
			if (ClassType == null)
				ClassType = TBabuClass.CreateType(aProperties);

			if (ClassType == null) return null;

			object bc = Activator.CreateInstance(ClassType);
			return (TBabuClass)bc;
		}

		/// <summary>
		/// 使用內部已存在的 ClassType 產生一個實例
		/// 如果內部 ClsssType 不存在，傳回 null
		/// </summary>
		/// <returns></returns>
		public TBabuClass CreateInstance()
		{
			if (ClassType == null)
				return null;

			object bc = Activator.CreateInstance(ClassType);
			return (TBabuClass)bc;
		}

		/// <summary>
		/// 產生一個以 Dictionary 的 Key 為 Property, Value 為值的動態類別實例
		/// </summary>
		/// <param name="aPropertyValue">要拿來當 Property 的內容</param>
		/// <returns>動態產生的類別</returns>
		static public TBabuClass CreateTypeAndInstance(Dictionary<string, string> aPropertyValue)
		{
			Type myType = TBabuClass.CreateType(aPropertyValue);
			TBabuClass bc = (TBabuClass)Activator.CreateInstance(myType);
			TBabuTools.FillProperty(bc, aPropertyValue);
			return bc;
		}

		static public TBabuClass CreateTypeAndInstance(string [] aProperties)
		{
			Type myType = TBabuClass.CreateType(aProperties);
			TBabuClass bc = (TBabuClass)Activator.CreateInstance(myType);
			return bc;
		}

		static public TBabuClass CreateInstance(Type aType)
		{
			return (TBabuClass)Activator.CreateInstance(aType);
		}

		/// <summary>
		/// 產生一個以 Dictionary 的 Key 為 Property 的動態類別型態
		/// </summary>
		/// <param name="aPropertyValue">要拿來當 Property 的內容</param>
		/// <returns></returns>
		public static Type CreateType(Dictionary<string, string> aPropertyValue)
		{
			return TBabuTools.CreateType("BabuRuntimeClass", aPropertyValue);
		}

		public static Type CreateType(string [] aProperties)
		{
			return TBabuTools.CreateType("BabuRuntimeClass", aProperties);
		}

		/// <summary>
		/// 將 aSource 的屬性內容逐一設定過來
		/// </summary>
		/// <param name="aSource">要複製屬性值的來源</param>
		public void Assign(TBabuClass aSource)
		{
			Type myType = this.GetType();
			PropertyInfo[] pis = aSource.GetType().GetProperties();
			PropertyInfo pi = null;

			for (int i = 0; i < pis.Length; i++)
			{
				pi = myType.GetProperty(pis[i].Name);

				if (pi == null || !pi.CanWrite || pi.PropertyType.ToString()!="System.String") 
					continue;

				try
				{
					this[pi.Name] = aSource[pis[i].Name];
				}
				catch
				{
				}
			}

			pis = null;
			pi = null;
			myType = null;
		}

		/// <summary>
		/// 新增 Property 到 Babu 裡面，並傳回新實體
		/// </summary>
		/// <param name="aProperties">要新增的 Property</param>
		/// <param name="aPropertiesType">要新增的 Property Type</param>
		/// <returns></returns>
		public TBabuClass AppendProperties(string[] aProperties, Type[] aPropertiesType)
		{
			Type NewType = TBabuTools.CreateTypeFrom(this.GetType(), aProperties, aPropertiesType);

			TBabuClass bc = (TBabuClass)Activator.CreateInstance(NewType);
			bc.Assign(this);
			return bc;
		}

		/// <summary>
		/// 取得 Property (object 型態)
		/// </summary>
		/// <param name="aPropertyName"></param>
		/// <returns></returns>
		public object GetProperty(string aPropertyName)
		{
			PropertyInfo pi = this.GetType().GetProperty(aPropertyName);
			if (pi != null)
				return pi.GetValue(this, null);
			else
				return null;
		}

		/// <summary>
		/// 設定 Property 的值
		/// </summary>
		/// <param name="aPropertyName">Property Name</param>
		/// <param name="aValue">Value</param>
		public void SetPropertyValue(string aPropertyName, object aValue)
		{
			PropertyInfo pi = this.GetType().GetProperty(aPropertyName);
			if (pi != null)
				pi.SetValue(this, aValue, null);
		}
	}
}
