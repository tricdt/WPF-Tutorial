#region Copyright (c) 2000-2024 Developer Express Inc.
/*
{*******************************************************************}
{                                                                   }
{       Developer Express .NET Component Library                    }
{                                                                   }
{                                                                   }
{       Copyright (c) 2000-2024 Developer Express Inc.              }
{       ALL RIGHTS RESERVED                                         }
{                                                                   }
{   The entire contents of this file is protected by U.S. and       }
{   International Copyright Laws. Unauthorized reproduction,        }
{   reverse-engineering, and distribution of all or any portion of  }
{   the code contained in this file is strictly prohibited and may  }
{   result in severe civil and criminal penalties and will be       }
{   prosecuted to the maximum extent possible under the law.        }
{                                                                   }
{   RESTRICTIONS                                                    }
{                                                                   }
{   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           }
{   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          }
{   SECRETS OF DEVELOPER EXPRESS INC. THE REGISTERED DEVELOPER IS   }
{   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    }
{   CONTROLS AS PART OF AN EXECUTABLE PROGRAM ONLY.                 }
{                                                                   }
{   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      }
{   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        }
{   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       }
{   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  }
{   AND PERMISSION FROM DEVELOPER EXPRESS INC.                      }
{                                                                   }
{   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       }
{   ADDITIONAL RESTRICTIONS.                                        }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2000-2024 Developer Express Inc.

#pragma warning disable DX0008
namespace DevExpress.Xpf.DemoBase.Native.Serialization.GeneratedSerializers.CarsData {
	public class XmlSerializationWriterCarsData : System.Xml.Serialization.XmlSerializationWriter {
		public void Write3_NewDataSet(object o) {
			WriteStartDocument();
			if (o == null) {
				WriteNullTagLiteral(@"NewDataSet", @"");
				return;
			}
			TopLevelElement();
			{
				global::DevExpress.Xpf.DemoBase.DataClasses.CarsData a = (global::DevExpress.Xpf.DemoBase.DataClasses.CarsData)((global::DevExpress.Xpf.DemoBase.DataClasses.CarsData)o);
				if ((object)(a) == null) {
					WriteNullTagLiteral(@"NewDataSet", @"");
				}
				else {
					WriteStartElement(@"NewDataSet", @"", null, false);
					for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
						Write2_Cars(@"Cars", @"", ((global::DevExpress.Xpf.DemoBase.DataClasses.Cars)a[ia]), true, false);
					}
					WriteEndElement();
				}
			}
		}
		void Write2_Cars(string n, string ns, global::DevExpress.Xpf.DemoBase.DataClasses.Cars o, bool isNullable, bool needType) {
			if ((object)o == null) {
				if (isNullable) WriteNullTagLiteral(n, ns);
				return;
			}
			if (!needType) {
				System.Type t = o.GetType();
				if (t == typeof(global::DevExpress.Xpf.DemoBase.DataClasses.Cars)) {
				}
				else {
					throw CreateUnknownTypeException(o);
				}
			}
			WriteStartElement(n, ns, o, false, null);
			if (needType) WriteXsiType(@"Cars", @"");
			WriteElementStringRaw(@"ID", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@ID)));
			WriteElementString(@"Trademark", @"", ((global::System.String)o.@Trademark));
			WriteElementString(@"Model", @"", ((global::System.String)o.@Model));
			WriteElementStringRaw(@"HP", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@HP)));
			WriteElementStringRaw(@"Liter", @"", System.Xml.XmlConvert.ToString((global::System.Double)((global::System.Double)o.@Liter)));
			WriteElementStringRaw(@"Cyl", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@Cyl)));
			WriteElementStringRaw(@"Transmiss_x0020_Speed_x0020_Count", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@TransmissSpeedCount)));
			WriteElementString(@"Transmiss_x0020_Automatic", @"", ((global::System.String)o.@TransmissAutomatic));
			WriteElementStringRaw(@"MPGCity", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@MPGCity)));
			WriteElementStringRaw(@"MPGHighway", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@MPGHighway)));
			WriteElementString(@"Category", @"", ((global::System.String)o.@Category));
			WriteElementString(@"Description", @"", ((global::System.String)o.@Description));
			WriteElementString(@"Hyperlink", @"", ((global::System.String)o.@Hyperlink));
			WriteElementStringRaw(@"Picture", @"", FromByteArrayBase64(((global::System.Byte[])o.@Picture)));
			WriteElementStringRaw(@"Price", @"", System.Xml.XmlConvert.ToString((global::System.Decimal)((global::System.Decimal)o.@Price)));
			WriteElementStringRaw(@"Delivery_x0020_Date", @"", FromDateTime(((global::System.DateTime)o.@DeliveryDate)));
			WriteElementStringRaw(@"Is_x0020_In_x0020_Stock", @"", System.Xml.XmlConvert.ToString((global::System.Boolean)((global::System.Boolean)o.@IsInStock)));
			WriteEndElement(o);
		}
		protected override void InitCallbacks() {
		}
	}
	public class XmlSerializationReaderCarsData : System.Xml.Serialization.XmlSerializationReader {
		public object Read3_NewDataSet() {
			object o = null;
			Reader.MoveToContent();
			if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
				if (((object) Reader.LocalName == (object)id1_NewDataSet && (object) Reader.NamespaceURI == (object)id2_Item)) {
					if (!ReadNull()) {
						if ((object)(o) == null) o = new global::DevExpress.Xpf.DemoBase.DataClasses.CarsData();
						global::DevExpress.Xpf.DemoBase.DataClasses.CarsData a_0_0 = (global::DevExpress.Xpf.DemoBase.DataClasses.CarsData)o;
						if ((Reader.IsEmptyElement)) {
							Reader.Skip();
						}
						else {
							Reader.ReadStartElement();
							Reader.MoveToContent();
							int whileIterations0 = 0;
							int readerCount0 = ReaderCount;
							while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
								if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
									if (((object) Reader.LocalName == (object)id3_Cars && (object) Reader.NamespaceURI == (object)id2_Item)) {
										if ((object)(a_0_0) == null) Reader.Skip(); else a_0_0.Add(Read2_Cars(true, true));
									}
									else {
										UnknownNode(null, @":Cars");
									}
								}
								else {
									UnknownNode(null, @":Cars");
								}
								Reader.MoveToContent();
								CheckReaderCount(ref whileIterations0, ref readerCount0);
							}
						ReadEndElement();
						}
					}
					else {
						if ((object)(o) == null) o = new global::DevExpress.Xpf.DemoBase.DataClasses.CarsData();
						global::DevExpress.Xpf.DemoBase.DataClasses.CarsData a_0_0 = (global::DevExpress.Xpf.DemoBase.DataClasses.CarsData)o;
					}
				}
				else {
					throw CreateUnknownNodeException();
				}
			}
			else {
				UnknownNode(null, @":NewDataSet");
			}
			return (object)o;
		}
		global::DevExpress.Xpf.DemoBase.DataClasses.Cars Read2_Cars(bool isNullable, bool checkType) {
			System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
			bool isNull = false;
			if (isNullable) isNull = ReadNull();
			if (checkType) {
			if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_Cars && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
			}
			else
				throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
			}
			if (isNull) return null;
			global::DevExpress.Xpf.DemoBase.DataClasses.Cars o;
			o = new global::DevExpress.Xpf.DemoBase.DataClasses.Cars();
			bool[] paramsRead = new bool[17];
			while (Reader.MoveToNextAttribute()) {
				if (!IsXmlnsAttribute(Reader.Name)) {
					UnknownNode((object)o);
				}
			}
			Reader.MoveToElement();
			if (Reader.IsEmptyElement) {
				Reader.Skip();
				return o;
			}
			Reader.ReadStartElement();
			Reader.MoveToContent();
			int whileIterations1 = 0;
			int readerCount1 = ReaderCount;
			while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
				if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
					if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_ID && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@ID = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
						}
						paramsRead[0] = true;
					}
					else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id5_Trademark && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Trademark = Reader.ReadElementString();
						}
						paramsRead[1] = true;
					}
					else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id6_Model && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Model = Reader.ReadElementString();
						}
						paramsRead[2] = true;
					}
					else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id7_HP && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@HP = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
						}
						paramsRead[3] = true;
					}
					else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id8_Liter && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Liter = System.Xml.XmlConvert.ToDouble(Reader.ReadElementString());
						}
						paramsRead[4] = true;
					}
					else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id9_Cyl && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Cyl = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
						}
						paramsRead[5] = true;
					}
					else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id10_Item && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@TransmissSpeedCount = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
						}
						paramsRead[6] = true;
					}
					else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id11_Transmiss_x0020_Automatic && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@TransmissAutomatic = Reader.ReadElementString();
						}
						paramsRead[7] = true;
					}
					else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id12_MPGCity && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@MPGCity = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
						}
						paramsRead[8] = true;
					}
					else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id13_MPGHighway && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@MPGHighway = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
						}
						paramsRead[9] = true;
					}
					else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id14_Category && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Category = Reader.ReadElementString();
						}
						paramsRead[10] = true;
					}
					else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id15_Description && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Description = Reader.ReadElementString();
						}
						paramsRead[11] = true;
					}
					else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id16_Hyperlink && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Hyperlink = Reader.ReadElementString();
						}
						paramsRead[12] = true;
					}
					else if (!paramsRead[13] && ((object) Reader.LocalName == (object)id17_Picture && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Picture = ToByteArrayBase64(false);
						}
						paramsRead[13] = true;
					}
					else if (!paramsRead[14] && ((object) Reader.LocalName == (object)id18_Price && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Price = System.Xml.XmlConvert.ToDecimal(Reader.ReadElementString());
						}
						paramsRead[14] = true;
					}
					else if (!paramsRead[15] && ((object) Reader.LocalName == (object)id19_Delivery_x0020_Date && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@DeliveryDate = ToDateTime(Reader.ReadElementString());
						}
						paramsRead[15] = true;
					}
					else if (!paramsRead[16] && ((object) Reader.LocalName == (object)id20_Is_x0020_In_x0020_Stock && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@IsInStock = System.Xml.XmlConvert.ToBoolean(Reader.ReadElementString());
						}
						paramsRead[16] = true;
					}
					else {
						UnknownNode((object)o, @":ID, :Trademark, :Model, :HP, :Liter, :Cyl, :Transmiss_x0020_Speed_x0020_Count, :Transmiss_x0020_Automatic, :MPGCity, :MPGHighway, :Category, :Description, :Hyperlink, :Picture, :Price, :Delivery_x0020_Date, :Is_x0020_In_x0020_Stock");
					}
				}
				else {
					UnknownNode((object)o, @":ID, :Trademark, :Model, :HP, :Liter, :Cyl, :Transmiss_x0020_Speed_x0020_Count, :Transmiss_x0020_Automatic, :MPGCity, :MPGHighway, :Category, :Description, :Hyperlink, :Picture, :Price, :Delivery_x0020_Date, :Is_x0020_In_x0020_Stock");
				}
				Reader.MoveToContent();
				CheckReaderCount(ref whileIterations1, ref readerCount1);
			}
			ReadEndElement();
			return o;
		}
		protected override void InitCallbacks() {
		}
		string id1_NewDataSet;
		string id19_Delivery_x0020_Date;
		string id7_HP;
		string id13_MPGHighway;
		string id9_Cyl;
		string id4_ID;
		string id15_Description;
		string id8_Liter;
		string id18_Price;
		string id20_Is_x0020_In_x0020_Stock;
		string id6_Model;
		string id17_Picture;
		string id12_MPGCity;
		string id11_Transmiss_x0020_Automatic;
		string id14_Category;
		string id2_Item;
		string id16_Hyperlink;
		string id10_Item;
		string id5_Trademark;
		string id3_Cars;
		protected override void InitIDs() {
			id1_NewDataSet = Reader.NameTable.Add(@"NewDataSet");
			id19_Delivery_x0020_Date = Reader.NameTable.Add(@"Delivery_x0020_Date");
			id7_HP = Reader.NameTable.Add(@"HP");
			id13_MPGHighway = Reader.NameTable.Add(@"MPGHighway");
			id9_Cyl = Reader.NameTable.Add(@"Cyl");
			id4_ID = Reader.NameTable.Add(@"ID");
			id15_Description = Reader.NameTable.Add(@"Description");
			id8_Liter = Reader.NameTable.Add(@"Liter");
			id18_Price = Reader.NameTable.Add(@"Price");
			id20_Is_x0020_In_x0020_Stock = Reader.NameTable.Add(@"Is_x0020_In_x0020_Stock");
			id6_Model = Reader.NameTable.Add(@"Model");
			id17_Picture = Reader.NameTable.Add(@"Picture");
			id12_MPGCity = Reader.NameTable.Add(@"MPGCity");
			id11_Transmiss_x0020_Automatic = Reader.NameTable.Add(@"Transmiss_x0020_Automatic");
			id14_Category = Reader.NameTable.Add(@"Category");
			id2_Item = Reader.NameTable.Add(@"");
			id16_Hyperlink = Reader.NameTable.Add(@"Hyperlink");
			id10_Item = Reader.NameTable.Add(@"Transmiss_x0020_Speed_x0020_Count");
			id5_Trademark = Reader.NameTable.Add(@"Trademark");
			id3_Cars = Reader.NameTable.Add(@"Cars");
		}
	}
	public abstract class XmlSerializer1 : System.Xml.Serialization.XmlSerializer {
		protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
			return new XmlSerializationReaderCarsData();
		}
		protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
			return new XmlSerializationWriterCarsData();
		}
	}
	public sealed class CarsDataSerializer : XmlSerializer1 {
		public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
			return xmlReader.IsStartElement(@"NewDataSet", @"");
		}
		protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
			((XmlSerializationWriterCarsData)writer).Write3_NewDataSet(objectToSerialize);
		}
		protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
			return ((XmlSerializationReaderCarsData)reader).Read3_NewDataSet();
		}
	}
	public class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
		public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new XmlSerializationReaderCarsData(); } }
		public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new XmlSerializationWriterCarsData(); } }
		System.Collections.Hashtable readMethods = null;
		public override System.Collections.Hashtable ReadMethods {
			get {
				if (readMethods == null) {
					System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
					_tmp[@"DevExpress.Xpf.DemoBase.DataClasses.CarsData::NewDataSet:True:"] = @"Read3_NewDataSet";
					if (readMethods == null) readMethods = _tmp;
				}
				return readMethods;
			}
		}
		System.Collections.Hashtable writeMethods = null;
		public override System.Collections.Hashtable WriteMethods {
			get {
				if (writeMethods == null) {
					System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
					_tmp[@"DevExpress.Xpf.DemoBase.DataClasses.CarsData::NewDataSet:True:"] = @"Write3_NewDataSet";
					if (writeMethods == null) writeMethods = _tmp;
				}
				return writeMethods;
			}
		}
		System.Collections.Hashtable typedSerializers = null;
		public override System.Collections.Hashtable TypedSerializers {
			get {
				if (typedSerializers == null) {
					System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
					_tmp.Add(@"DevExpress.Xpf.DemoBase.DataClasses.CarsData::NewDataSet:True:", new CarsDataSerializer());
					if (typedSerializers == null) typedSerializers = _tmp;
				}
				return typedSerializers;
			}
		}
		public override System.Boolean CanSerialize(System.Type type) {
			if (type == typeof(global::DevExpress.Xpf.DemoBase.DataClasses.CarsData)) return true;
			return false;
		}
		public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
			if (type == typeof(global::DevExpress.Xpf.DemoBase.DataClasses.CarsData)) return new CarsDataSerializer();
			return null;
		}
	}
}
namespace DevExpress.Xpf.DemoBase.Native.Serialization.GeneratedSerializers.EmployeesData {
	public class XmlSerializationWriterEmployeesData : System.Xml.Serialization.XmlSerializationWriter {
		public void Write3_Employees(object o) {
			WriteStartDocument();
			if (o == null) {
				WriteNullTagLiteral(@"Employees", @"");
				return;
			}
			TopLevelElement();
			{
				global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesData a = (global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesData)((global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesData)o);
				if ((object)(a) == null) {
					WriteNullTagLiteral(@"Employees", @"");
				}
				else {
					WriteStartElement(@"Employees", @"", null, false);
					for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
						Write2_Employee(@"Employee", @"", ((global::DevExpress.Xpf.DemoBase.DataClasses.Employee)a[ia]), true, false);
					}
					WriteEndElement();
				}
			}
		}
		void Write2_Employee(string n, string ns, global::DevExpress.Xpf.DemoBase.DataClasses.Employee o, bool isNullable, bool needType) {
			if ((object)o == null) {
				if (isNullable) WriteNullTagLiteral(n, ns);
				return;
			}
			if (!needType) {
				System.Type t = o.GetType();
				if (t == typeof(global::DevExpress.Xpf.DemoBase.DataClasses.Employee)) {
				}
				else {
					throw CreateUnknownTypeException(o);
				}
			}
			WriteStartElement(n, ns, o, false, null);
			if (needType) WriteXsiType(@"Employee", @"");
			WriteElementStringRaw(@"Id", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@Id)));
			WriteElementStringRaw(@"ParentId", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@ParentId)));
			WriteElementString(@"FirstName", @"", ((global::System.String)o.@FirstName));
			WriteElementString(@"MiddleName", @"", ((global::System.String)o.@MiddleName));
			WriteElementString(@"LastName", @"", ((global::System.String)o.@LastName));
			WriteElementString(@"JobTitle", @"", ((global::System.String)o.@JobTitle));
			WriteElementString(@"Phone", @"", ((global::System.String)o.@Phone));
			WriteElementString(@"EmailAddress", @"", ((global::System.String)o.@EmailAddress));
			WriteElementString(@"AddressLine1", @"", ((global::System.String)o.@AddressLine1));
			WriteElementString(@"City", @"", ((global::System.String)o.@City));
			WriteElementString(@"StateProvinceName", @"", ((global::System.String)o.@StateProvinceName));
			WriteElementString(@"PostalCode", @"", ((global::System.String)o.@PostalCode));
			WriteElementString(@"CountryRegionName", @"", ((global::System.String)o.@CountryRegionName));
			WriteElementString(@"GroupName", @"", ((global::System.String)o.@GroupName));
			WriteElementStringRaw(@"BirthDate", @"", FromDateTime(((global::System.DateTime)o.@BirthDate)));
			WriteElementStringRaw(@"HireDate", @"", FromDateTime(((global::System.DateTime)o.@HireDate)));
			WriteElementString(@"Gender", @"", ((global::System.String)o.@Gender));
			WriteElementString(@"MaritalStatus", @"", ((global::System.String)o.@MaritalStatus));
			WriteElementString(@"Title", @"", ((global::System.String)o.@Title));
			WriteElementStringRaw(@"ImageData", @"", FromByteArrayBase64(((global::System.Byte[])o.@ImageData)));
			WriteEndElement(o);
		}
		protected override void InitCallbacks() {
		}
	}
	public class XmlSerializationReaderEmployeesData : System.Xml.Serialization.XmlSerializationReader {
		public object Read3_Employees() {
			object o = null;
			Reader.MoveToContent();
			if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
				if (((object) Reader.LocalName == (object)id1_Employees && (object) Reader.NamespaceURI == (object)id2_Item)) {
					if (!ReadNull()) {
						if ((object)(o) == null) o = new global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesData();
						global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesData a_0_0 = (global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesData)o;
						if ((Reader.IsEmptyElement)) {
							Reader.Skip();
						}
						else {
							Reader.ReadStartElement();
							Reader.MoveToContent();
							int whileIterations0 = 0;
							int readerCount0 = ReaderCount;
							while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
								if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
									if (((object) Reader.LocalName == (object)id3_Employee && (object) Reader.NamespaceURI == (object)id2_Item)) {
										if ((object)(a_0_0) == null) Reader.Skip(); else a_0_0.Add(Read2_Employee(true, true));
									}
									else {
										UnknownNode(null, @":Employee");
									}
								}
								else {
									UnknownNode(null, @":Employee");
								}
								Reader.MoveToContent();
								CheckReaderCount(ref whileIterations0, ref readerCount0);
							}
						ReadEndElement();
						}
					}
					else {
						if ((object)(o) == null) o = new global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesData();
						global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesData a_0_0 = (global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesData)o;
					}
				}
				else {
					throw CreateUnknownNodeException();
				}
			}
			else {
				UnknownNode(null, @":Employees");
			}
			return (object)o;
		}
		global::DevExpress.Xpf.DemoBase.DataClasses.Employee Read2_Employee(bool isNullable, bool checkType) {
			System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
			bool isNull = false;
			if (isNullable) isNull = ReadNull();
			if (checkType) {
			if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_Employee && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
			}
			else
				throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
			}
			if (isNull) return null;
			global::DevExpress.Xpf.DemoBase.DataClasses.Employee o;
			o = new global::DevExpress.Xpf.DemoBase.DataClasses.Employee();
			bool[] paramsRead = new bool[20];
			while (Reader.MoveToNextAttribute()) {
				if (!IsXmlnsAttribute(Reader.Name)) {
					UnknownNode((object)o);
				}
			}
			Reader.MoveToElement();
			if (Reader.IsEmptyElement) {
				Reader.Skip();
				return o;
			}
			Reader.ReadStartElement();
			Reader.MoveToContent();
			int whileIterations1 = 0;
			int readerCount1 = ReaderCount;
			while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
				if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
					if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_Id && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Id = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
						}
						paramsRead[0] = true;
					}
					else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id5_ParentId && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@ParentId = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
						}
						paramsRead[1] = true;
					}
					else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id6_FirstName && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@FirstName = Reader.ReadElementString();
						}
						paramsRead[2] = true;
					}
					else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id7_MiddleName && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@MiddleName = Reader.ReadElementString();
						}
						paramsRead[3] = true;
					}
					else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id8_LastName && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@LastName = Reader.ReadElementString();
						}
						paramsRead[4] = true;
					}
					else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id9_JobTitle && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@JobTitle = Reader.ReadElementString();
						}
						paramsRead[5] = true;
					}
					else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id10_Phone && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Phone = Reader.ReadElementString();
						}
						paramsRead[6] = true;
					}
					else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id11_EmailAddress && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@EmailAddress = Reader.ReadElementString();
						}
						paramsRead[7] = true;
					}
					else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id12_AddressLine1 && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@AddressLine1 = Reader.ReadElementString();
						}
						paramsRead[8] = true;
					}
					else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id13_City && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@City = Reader.ReadElementString();
						}
						paramsRead[9] = true;
					}
					else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id14_StateProvinceName && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@StateProvinceName = Reader.ReadElementString();
						}
						paramsRead[10] = true;
					}
					else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id15_PostalCode && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@PostalCode = Reader.ReadElementString();
						}
						paramsRead[11] = true;
					}
					else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id16_CountryRegionName && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@CountryRegionName = Reader.ReadElementString();
						}
						paramsRead[12] = true;
					}
					else if (!paramsRead[13] && ((object) Reader.LocalName == (object)id17_GroupName && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@GroupName = Reader.ReadElementString();
						}
						paramsRead[13] = true;
					}
					else if (!paramsRead[14] && ((object) Reader.LocalName == (object)id18_BirthDate && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@BirthDate = ToDateTime(Reader.ReadElementString());
						}
						paramsRead[14] = true;
					}
					else if (!paramsRead[15] && ((object) Reader.LocalName == (object)id19_HireDate && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@HireDate = ToDateTime(Reader.ReadElementString());
						}
						paramsRead[15] = true;
					}
					else if (!paramsRead[16] && ((object) Reader.LocalName == (object)id20_Gender && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Gender = Reader.ReadElementString();
						}
						paramsRead[16] = true;
					}
					else if (!paramsRead[17] && ((object) Reader.LocalName == (object)id21_MaritalStatus && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@MaritalStatus = Reader.ReadElementString();
						}
						paramsRead[17] = true;
					}
					else if (!paramsRead[18] && ((object) Reader.LocalName == (object)id22_Title && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Title = Reader.ReadElementString();
						}
						paramsRead[18] = true;
					}
					else if (!paramsRead[19] && ((object) Reader.LocalName == (object)id23_ImageData && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@ImageData = ToByteArrayBase64(false);
						}
						paramsRead[19] = true;
					}
					else {
						UnknownNode((object)o, @":Id, :ParentId, :FirstName, :MiddleName, :LastName, :JobTitle, :Phone, :EmailAddress, :AddressLine1, :City, :StateProvinceName, :PostalCode, :CountryRegionName, :GroupName, :BirthDate, :HireDate, :Gender, :MaritalStatus, :Title, :ImageData");
					}
				}
				else {
					UnknownNode((object)o, @":Id, :ParentId, :FirstName, :MiddleName, :LastName, :JobTitle, :Phone, :EmailAddress, :AddressLine1, :City, :StateProvinceName, :PostalCode, :CountryRegionName, :GroupName, :BirthDate, :HireDate, :Gender, :MaritalStatus, :Title, :ImageData");
				}
				Reader.MoveToContent();
				CheckReaderCount(ref whileIterations1, ref readerCount1);
			}
			ReadEndElement();
			return o;
		}
		protected override void InitCallbacks() {
		}
		string id10_Phone;
		string id11_EmailAddress;
		string id21_MaritalStatus;
		string id18_BirthDate;
		string id8_LastName;
		string id16_CountryRegionName;
		string id3_Employee;
		string id20_Gender;
		string id12_AddressLine1;
		string id15_PostalCode;
		string id4_Id;
		string id22_Title;
		string id6_FirstName;
		string id1_Employees;
		string id23_ImageData;
		string id2_Item;
		string id14_StateProvinceName;
		string id7_MiddleName;
		string id9_JobTitle;
		string id19_HireDate;
		string id17_GroupName;
		string id13_City;
		string id5_ParentId;
		protected override void InitIDs() {
			id10_Phone = Reader.NameTable.Add(@"Phone");
			id11_EmailAddress = Reader.NameTable.Add(@"EmailAddress");
			id21_MaritalStatus = Reader.NameTable.Add(@"MaritalStatus");
			id18_BirthDate = Reader.NameTable.Add(@"BirthDate");
			id8_LastName = Reader.NameTable.Add(@"LastName");
			id16_CountryRegionName = Reader.NameTable.Add(@"CountryRegionName");
			id3_Employee = Reader.NameTable.Add(@"Employee");
			id20_Gender = Reader.NameTable.Add(@"Gender");
			id12_AddressLine1 = Reader.NameTable.Add(@"AddressLine1");
			id15_PostalCode = Reader.NameTable.Add(@"PostalCode");
			id4_Id = Reader.NameTable.Add(@"Id");
			id22_Title = Reader.NameTable.Add(@"Title");
			id6_FirstName = Reader.NameTable.Add(@"FirstName");
			id1_Employees = Reader.NameTable.Add(@"Employees");
			id23_ImageData = Reader.NameTable.Add(@"ImageData");
			id2_Item = Reader.NameTable.Add(@"");
			id14_StateProvinceName = Reader.NameTable.Add(@"StateProvinceName");
			id7_MiddleName = Reader.NameTable.Add(@"MiddleName");
			id9_JobTitle = Reader.NameTable.Add(@"JobTitle");
			id19_HireDate = Reader.NameTable.Add(@"HireDate");
			id17_GroupName = Reader.NameTable.Add(@"GroupName");
			id13_City = Reader.NameTable.Add(@"City");
			id5_ParentId = Reader.NameTable.Add(@"ParentId");
		}
	}
	public abstract class XmlSerializer1 : System.Xml.Serialization.XmlSerializer {
		protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
			return new XmlSerializationReaderEmployeesData();
		}
		protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
			return new XmlSerializationWriterEmployeesData();
		}
	}
	public sealed class EmployeesDataSerializer : XmlSerializer1 {
		public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
			return xmlReader.IsStartElement(@"Employees", @"");
		}
		protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
			((XmlSerializationWriterEmployeesData)writer).Write3_Employees(objectToSerialize);
		}
		protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
			return ((XmlSerializationReaderEmployeesData)reader).Read3_Employees();
		}
	}
	public class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
		public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new XmlSerializationReaderEmployeesData(); } }
		public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new XmlSerializationWriterEmployeesData(); } }
		System.Collections.Hashtable readMethods = null;
		public override System.Collections.Hashtable ReadMethods {
			get {
				if (readMethods == null) {
					System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
					_tmp[@"DevExpress.Xpf.DemoBase.DataClasses.EmployeesData::Employees:True:"] = @"Read3_Employees";
					if (readMethods == null) readMethods = _tmp;
				}
				return readMethods;
			}
		}
		System.Collections.Hashtable writeMethods = null;
		public override System.Collections.Hashtable WriteMethods {
			get {
				if (writeMethods == null) {
					System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
					_tmp[@"DevExpress.Xpf.DemoBase.DataClasses.EmployeesData::Employees:True:"] = @"Write3_Employees";
					if (writeMethods == null) writeMethods = _tmp;
				}
				return writeMethods;
			}
		}
		System.Collections.Hashtable typedSerializers = null;
		public override System.Collections.Hashtable TypedSerializers {
			get {
				if (typedSerializers == null) {
					System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
					_tmp.Add(@"DevExpress.Xpf.DemoBase.DataClasses.EmployeesData::Employees:True:", new EmployeesDataSerializer());
					if (typedSerializers == null) typedSerializers = _tmp;
				}
				return typedSerializers;
			}
		}
		public override System.Boolean CanSerialize(System.Type type) {
			if (type == typeof(global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesData)) return true;
			return false;
		}
		public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
			if (type == typeof(global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesData)) return new EmployeesDataSerializer();
			return null;
		}
	}
}
namespace DevExpress.Xpf.DemoBase.Native.Serialization.GeneratedSerializers.EmployeesWithPhotoData {
	public class XmlSerializationWriterEmployeesWithPhotoData : System.Xml.Serialization.XmlSerializationWriter {
		public void Write3_Employees(object o) {
			WriteStartDocument();
			if (o == null) {
				WriteNullTagLiteral(@"Employees", @"");
				return;
			}
			TopLevelElement();
			{
				global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesWithPhotoData a = (global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesWithPhotoData)((global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesWithPhotoData)o);
				if ((object)(a) == null) {
					WriteNullTagLiteral(@"Employees", @"");
				}
				else {
					WriteStartElement(@"Employees", @"", null, false);
					for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++) {
						Write2_Employee(@"Employee", @"", ((global::DevExpress.Xpf.DemoBase.DataClasses.Employee)a[ia]), true, false);
					}
					WriteEndElement();
				}
			}
		}
		void Write2_Employee(string n, string ns, global::DevExpress.Xpf.DemoBase.DataClasses.Employee o, bool isNullable, bool needType) {
			if ((object)o == null) {
				if (isNullable) WriteNullTagLiteral(n, ns);
				return;
			}
			if (!needType) {
				System.Type t = o.GetType();
				if (t == typeof(global::DevExpress.Xpf.DemoBase.DataClasses.Employee)) {
				}
				else {
					throw CreateUnknownTypeException(o);
				}
			}
			WriteStartElement(n, ns, o, false, null);
			if (needType) WriteXsiType(@"Employee", @"");
			WriteElementStringRaw(@"Id", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@Id)));
			WriteElementStringRaw(@"ParentId", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@ParentId)));
			WriteElementString(@"FirstName", @"", ((global::System.String)o.@FirstName));
			WriteElementString(@"MiddleName", @"", ((global::System.String)o.@MiddleName));
			WriteElementString(@"LastName", @"", ((global::System.String)o.@LastName));
			WriteElementString(@"JobTitle", @"", ((global::System.String)o.@JobTitle));
			WriteElementString(@"Phone", @"", ((global::System.String)o.@Phone));
			WriteElementString(@"EmailAddress", @"", ((global::System.String)o.@EmailAddress));
			WriteElementString(@"AddressLine1", @"", ((global::System.String)o.@AddressLine1));
			WriteElementString(@"City", @"", ((global::System.String)o.@City));
			WriteElementString(@"StateProvinceName", @"", ((global::System.String)o.@StateProvinceName));
			WriteElementString(@"PostalCode", @"", ((global::System.String)o.@PostalCode));
			WriteElementString(@"CountryRegionName", @"", ((global::System.String)o.@CountryRegionName));
			WriteElementString(@"GroupName", @"", ((global::System.String)o.@GroupName));
			WriteElementStringRaw(@"BirthDate", @"", FromDateTime(((global::System.DateTime)o.@BirthDate)));
			WriteElementStringRaw(@"HireDate", @"", FromDateTime(((global::System.DateTime)o.@HireDate)));
			WriteElementString(@"Gender", @"", ((global::System.String)o.@Gender));
			WriteElementString(@"MaritalStatus", @"", ((global::System.String)o.@MaritalStatus));
			WriteElementString(@"Title", @"", ((global::System.String)o.@Title));
			WriteElementStringRaw(@"ImageData", @"", FromByteArrayBase64(((global::System.Byte[])o.@ImageData)));
			WriteEndElement(o);
		}
		protected override void InitCallbacks() {
		}
	}
	public class XmlSerializationReaderEmployeesWithPhotoData : System.Xml.Serialization.XmlSerializationReader {
		public object Read3_Employees() {
			object o = null;
			Reader.MoveToContent();
			if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
				if (((object) Reader.LocalName == (object)id1_Employees && (object) Reader.NamespaceURI == (object)id2_Item)) {
					if (!ReadNull()) {
						if ((object)(o) == null) o = new global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesWithPhotoData();
						global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesWithPhotoData a_0_0 = (global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesWithPhotoData)o;
						if ((Reader.IsEmptyElement)) {
							Reader.Skip();
						}
						else {
							Reader.ReadStartElement();
							Reader.MoveToContent();
							int whileIterations0 = 0;
							int readerCount0 = ReaderCount;
							while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
								if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
									if (((object) Reader.LocalName == (object)id3_Employee && (object) Reader.NamespaceURI == (object)id2_Item)) {
										if ((object)(a_0_0) == null) Reader.Skip(); else a_0_0.Add(Read2_Employee(true, true));
									}
									else {
										UnknownNode(null, @":Employee");
									}
								}
								else {
									UnknownNode(null, @":Employee");
								}
								Reader.MoveToContent();
								CheckReaderCount(ref whileIterations0, ref readerCount0);
							}
						ReadEndElement();
						}
					}
					else {
						if ((object)(o) == null) o = new global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesWithPhotoData();
						global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesWithPhotoData a_0_0 = (global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesWithPhotoData)o;
					}
				}
				else {
					throw CreateUnknownNodeException();
				}
			}
			else {
				UnknownNode(null, @":Employees");
			}
			return (object)o;
		}
		global::DevExpress.Xpf.DemoBase.DataClasses.Employee Read2_Employee(bool isNullable, bool checkType) {
			System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
			bool isNull = false;
			if (isNullable) isNull = ReadNull();
			if (checkType) {
			if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_Employee && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
			}
			else
				throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
			}
			if (isNull) return null;
			global::DevExpress.Xpf.DemoBase.DataClasses.Employee o;
			o = new global::DevExpress.Xpf.DemoBase.DataClasses.Employee();
			bool[] paramsRead = new bool[20];
			while (Reader.MoveToNextAttribute()) {
				if (!IsXmlnsAttribute(Reader.Name)) {
					UnknownNode((object)o);
				}
			}
			Reader.MoveToElement();
			if (Reader.IsEmptyElement) {
				Reader.Skip();
				return o;
			}
			Reader.ReadStartElement();
			Reader.MoveToContent();
			int whileIterations1 = 0;
			int readerCount1 = ReaderCount;
			while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
				if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
					if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_Id && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Id = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
						}
						paramsRead[0] = true;
					}
					else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id5_ParentId && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@ParentId = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
						}
						paramsRead[1] = true;
					}
					else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id6_FirstName && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@FirstName = Reader.ReadElementString();
						}
						paramsRead[2] = true;
					}
					else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id7_MiddleName && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@MiddleName = Reader.ReadElementString();
						}
						paramsRead[3] = true;
					}
					else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id8_LastName && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@LastName = Reader.ReadElementString();
						}
						paramsRead[4] = true;
					}
					else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id9_JobTitle && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@JobTitle = Reader.ReadElementString();
						}
						paramsRead[5] = true;
					}
					else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id10_Phone && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Phone = Reader.ReadElementString();
						}
						paramsRead[6] = true;
					}
					else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id11_EmailAddress && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@EmailAddress = Reader.ReadElementString();
						}
						paramsRead[7] = true;
					}
					else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id12_AddressLine1 && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@AddressLine1 = Reader.ReadElementString();
						}
						paramsRead[8] = true;
					}
					else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id13_City && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@City = Reader.ReadElementString();
						}
						paramsRead[9] = true;
					}
					else if (!paramsRead[10] && ((object) Reader.LocalName == (object)id14_StateProvinceName && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@StateProvinceName = Reader.ReadElementString();
						}
						paramsRead[10] = true;
					}
					else if (!paramsRead[11] && ((object) Reader.LocalName == (object)id15_PostalCode && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@PostalCode = Reader.ReadElementString();
						}
						paramsRead[11] = true;
					}
					else if (!paramsRead[12] && ((object) Reader.LocalName == (object)id16_CountryRegionName && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@CountryRegionName = Reader.ReadElementString();
						}
						paramsRead[12] = true;
					}
					else if (!paramsRead[13] && ((object) Reader.LocalName == (object)id17_GroupName && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@GroupName = Reader.ReadElementString();
						}
						paramsRead[13] = true;
					}
					else if (!paramsRead[14] && ((object) Reader.LocalName == (object)id18_BirthDate && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@BirthDate = ToDateTime(Reader.ReadElementString());
						}
						paramsRead[14] = true;
					}
					else if (!paramsRead[15] && ((object) Reader.LocalName == (object)id19_HireDate && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@HireDate = ToDateTime(Reader.ReadElementString());
						}
						paramsRead[15] = true;
					}
					else if (!paramsRead[16] && ((object) Reader.LocalName == (object)id20_Gender && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Gender = Reader.ReadElementString();
						}
						paramsRead[16] = true;
					}
					else if (!paramsRead[17] && ((object) Reader.LocalName == (object)id21_MaritalStatus && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@MaritalStatus = Reader.ReadElementString();
						}
						paramsRead[17] = true;
					}
					else if (!paramsRead[18] && ((object) Reader.LocalName == (object)id22_Title && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Title = Reader.ReadElementString();
						}
						paramsRead[18] = true;
					}
					else if (!paramsRead[19] && ((object) Reader.LocalName == (object)id23_ImageData && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@ImageData = ToByteArrayBase64(false);
						}
						paramsRead[19] = true;
					}
					else {
						UnknownNode((object)o, @":Id, :ParentId, :FirstName, :MiddleName, :LastName, :JobTitle, :Phone, :EmailAddress, :AddressLine1, :City, :StateProvinceName, :PostalCode, :CountryRegionName, :GroupName, :BirthDate, :HireDate, :Gender, :MaritalStatus, :Title, :ImageData");
					}
				}
				else {
					UnknownNode((object)o, @":Id, :ParentId, :FirstName, :MiddleName, :LastName, :JobTitle, :Phone, :EmailAddress, :AddressLine1, :City, :StateProvinceName, :PostalCode, :CountryRegionName, :GroupName, :BirthDate, :HireDate, :Gender, :MaritalStatus, :Title, :ImageData");
				}
				Reader.MoveToContent();
				CheckReaderCount(ref whileIterations1, ref readerCount1);
			}
			ReadEndElement();
			return o;
		}
		protected override void InitCallbacks() {
		}
		string id10_Phone;
		string id11_EmailAddress;
		string id21_MaritalStatus;
		string id18_BirthDate;
		string id8_LastName;
		string id16_CountryRegionName;
		string id3_Employee;
		string id20_Gender;
		string id12_AddressLine1;
		string id15_PostalCode;
		string id4_Id;
		string id22_Title;
		string id6_FirstName;
		string id1_Employees;
		string id23_ImageData;
		string id2_Item;
		string id14_StateProvinceName;
		string id7_MiddleName;
		string id9_JobTitle;
		string id19_HireDate;
		string id17_GroupName;
		string id13_City;
		string id5_ParentId;
		protected override void InitIDs() {
			id10_Phone = Reader.NameTable.Add(@"Phone");
			id11_EmailAddress = Reader.NameTable.Add(@"EmailAddress");
			id21_MaritalStatus = Reader.NameTable.Add(@"MaritalStatus");
			id18_BirthDate = Reader.NameTable.Add(@"BirthDate");
			id8_LastName = Reader.NameTable.Add(@"LastName");
			id16_CountryRegionName = Reader.NameTable.Add(@"CountryRegionName");
			id3_Employee = Reader.NameTable.Add(@"Employee");
			id20_Gender = Reader.NameTable.Add(@"Gender");
			id12_AddressLine1 = Reader.NameTable.Add(@"AddressLine1");
			id15_PostalCode = Reader.NameTable.Add(@"PostalCode");
			id4_Id = Reader.NameTable.Add(@"Id");
			id22_Title = Reader.NameTable.Add(@"Title");
			id6_FirstName = Reader.NameTable.Add(@"FirstName");
			id1_Employees = Reader.NameTable.Add(@"Employees");
			id23_ImageData = Reader.NameTable.Add(@"ImageData");
			id2_Item = Reader.NameTable.Add(@"");
			id14_StateProvinceName = Reader.NameTable.Add(@"StateProvinceName");
			id7_MiddleName = Reader.NameTable.Add(@"MiddleName");
			id9_JobTitle = Reader.NameTable.Add(@"JobTitle");
			id19_HireDate = Reader.NameTable.Add(@"HireDate");
			id17_GroupName = Reader.NameTable.Add(@"GroupName");
			id13_City = Reader.NameTable.Add(@"City");
			id5_ParentId = Reader.NameTable.Add(@"ParentId");
		}
	}
	public abstract class XmlSerializer1 : System.Xml.Serialization.XmlSerializer {
		protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
			return new XmlSerializationReaderEmployeesWithPhotoData();
		}
		protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
			return new XmlSerializationWriterEmployeesWithPhotoData();
		}
	}
	public sealed class EmployeesWithPhotoDataSerializer : XmlSerializer1 {
		public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
			return xmlReader.IsStartElement(@"Employees", @"");
		}
		protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
			((XmlSerializationWriterEmployeesWithPhotoData)writer).Write3_Employees(objectToSerialize);
		}
		protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
			return ((XmlSerializationReaderEmployeesWithPhotoData)reader).Read3_Employees();
		}
	}
	public class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
		public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new XmlSerializationReaderEmployeesWithPhotoData(); } }
		public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new XmlSerializationWriterEmployeesWithPhotoData(); } }
		System.Collections.Hashtable readMethods = null;
		public override System.Collections.Hashtable ReadMethods {
			get {
				if (readMethods == null) {
					System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
					_tmp[@"DevExpress.Xpf.DemoBase.DataClasses.EmployeesWithPhotoData::Employees:True:"] = @"Read3_Employees";
					if (readMethods == null) readMethods = _tmp;
				}
				return readMethods;
			}
		}
		System.Collections.Hashtable writeMethods = null;
		public override System.Collections.Hashtable WriteMethods {
			get {
				if (writeMethods == null) {
					System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
					_tmp[@"DevExpress.Xpf.DemoBase.DataClasses.EmployeesWithPhotoData::Employees:True:"] = @"Write3_Employees";
					if (writeMethods == null) writeMethods = _tmp;
				}
				return writeMethods;
			}
		}
		System.Collections.Hashtable typedSerializers = null;
		public override System.Collections.Hashtable TypedSerializers {
			get {
				if (typedSerializers == null) {
					System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
					_tmp.Add(@"DevExpress.Xpf.DemoBase.DataClasses.EmployeesWithPhotoData::Employees:True:", new EmployeesWithPhotoDataSerializer());
					if (typedSerializers == null) typedSerializers = _tmp;
				}
				return typedSerializers;
			}
		}
		public override System.Boolean CanSerialize(System.Type type) {
			if (type == typeof(global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesWithPhotoData)) return true;
			return false;
		}
		public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
			if (type == typeof(global::DevExpress.Xpf.DemoBase.DataClasses.EmployeesWithPhotoData)) return new EmployeesWithPhotoDataSerializer();
			return null;
		}
	}
}
namespace DevExpress.Xpf.DemoBase.Native.Serialization.GeneratedSerializers.NWindOrderToNewEmployeeArray {
	public class XmlSerializationWriterNWindOrderToNewEmployeeArray : System.Xml.Serialization.XmlSerializationWriter {
		public void Write3_ArrayOfNWindOrderToNewEmployee(object o) {
			WriteStartDocument();
			if (o == null) {
				WriteNullTagLiteral(@"ArrayOfNWindOrderToNewEmployee", @"");
				return;
			}
			TopLevelElement();
			{
				global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee[] a = (global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee[])((global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee[])o);
				if ((object)(a) == null) {
					WriteNullTagLiteral(@"ArrayOfNWindOrderToNewEmployee", @"");
				}
				else {
					WriteStartElement(@"ArrayOfNWindOrderToNewEmployee", @"", null, false);
					for (int ia = 0; ia < a.Length; ia++) {
						Write2_NWindOrderToNewEmployee(@"NWindOrderToNewEmployee", @"", ((global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee)a[ia]), true, false);
					}
					WriteEndElement();
				}
			}
		}
		void Write2_NWindOrderToNewEmployee(string n, string ns, global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee o, bool isNullable, bool needType) {
			if ((object)o == null) {
				if (isNullable) WriteNullTagLiteral(n, ns);
				return;
			}
			if (!needType) {
				System.Type t = o.GetType();
				if (t == typeof(global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee)) {
				}
				else {
					throw CreateUnknownTypeException(o);
				}
			}
			WriteStartElement(n, ns, o, false, null);
			if (needType) WriteXsiType(@"NWindOrderToNewEmployee", @"");
			WriteElementStringRaw(@"NWindOrderId", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@NWindOrderId)));
			WriteElementStringRaw(@"EmployeeId", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@EmployeeId)));
			WriteEndElement(o);
		}
		protected override void InitCallbacks() {
		}
	}
	public class XmlSerializationReaderNWindOrderToNewEmployeeArray : System.Xml.Serialization.XmlSerializationReader {
		public object Read3_ArrayOfNWindOrderToNewEmployee() {
			object o = null;
			Reader.MoveToContent();
			if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
				if (((object) Reader.LocalName == (object)id1_ArrayOfNWindOrderToNewEmployee && (object) Reader.NamespaceURI == (object)id2_Item)) {
					if (!ReadNull()) {
						global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee[] a_0_0 = null;
						int ca_0_0 = 0;
						if ((Reader.IsEmptyElement)) {
							Reader.Skip();
						}
						else {
							Reader.ReadStartElement();
							Reader.MoveToContent();
							int whileIterations0 = 0;
							int readerCount0 = ReaderCount;
							while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
								if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
									if (((object) Reader.LocalName == (object)id3_NWindOrderToNewEmployee && (object) Reader.NamespaceURI == (object)id2_Item)) {
										a_0_0 = (global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee[])EnsureArrayIndex(a_0_0, ca_0_0, typeof(global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee));a_0_0[ca_0_0++] = Read2_NWindOrderToNewEmployee(true, true);
									}
									else {
										UnknownNode(null, @":NWindOrderToNewEmployee");
									}
								}
								else {
									UnknownNode(null, @":NWindOrderToNewEmployee");
								}
								Reader.MoveToContent();
								CheckReaderCount(ref whileIterations0, ref readerCount0);
							}
						ReadEndElement();
						}
						o = (global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee[])ShrinkArray(a_0_0, ca_0_0, typeof(global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee), false);
					}
					else {
						global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee[] a_0_0 = null;
						int ca_0_0 = 0;
						o = (global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee[])ShrinkArray(a_0_0, ca_0_0, typeof(global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee), true);
					}
				}
				else {
					throw CreateUnknownNodeException();
				}
			}
			else {
				UnknownNode(null, @":ArrayOfNWindOrderToNewEmployee");
			}
			return (object)o;
		}
		global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee Read2_NWindOrderToNewEmployee(bool isNullable, bool checkType) {
			System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
			bool isNull = false;
			if (isNullable) isNull = ReadNull();
			if (checkType) {
			if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_NWindOrderToNewEmployee && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
			}
			else
				throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
			}
			if (isNull) return null;
			global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee o;
			o = new global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee();
			bool[] paramsRead = new bool[2];
			while (Reader.MoveToNextAttribute()) {
				if (!IsXmlnsAttribute(Reader.Name)) {
					UnknownNode((object)o);
				}
			}
			Reader.MoveToElement();
			if (Reader.IsEmptyElement) {
				Reader.Skip();
				return o;
			}
			Reader.ReadStartElement();
			Reader.MoveToContent();
			int whileIterations1 = 0;
			int readerCount1 = ReaderCount;
			while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
				if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
					if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_NWindOrderId && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@NWindOrderId = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
						}
						paramsRead[0] = true;
					}
					else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id5_EmployeeId && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@EmployeeId = System.Xml.XmlConvert.ToInt32(Reader.ReadElementString());
						}
						paramsRead[1] = true;
					}
					else {
						UnknownNode((object)o, @":NWindOrderId, :EmployeeId");
					}
				}
				else {
					UnknownNode((object)o, @":NWindOrderId, :EmployeeId");
				}
				Reader.MoveToContent();
				CheckReaderCount(ref whileIterations1, ref readerCount1);
			}
			ReadEndElement();
			return o;
		}
		protected override void InitCallbacks() {
		}
		string id1_ArrayOfNWindOrderToNewEmployee;
		string id4_NWindOrderId;
		string id5_EmployeeId;
		string id3_NWindOrderToNewEmployee;
		string id2_Item;
		protected override void InitIDs() {
			id1_ArrayOfNWindOrderToNewEmployee = Reader.NameTable.Add(@"ArrayOfNWindOrderToNewEmployee");
			id4_NWindOrderId = Reader.NameTable.Add(@"NWindOrderId");
			id5_EmployeeId = Reader.NameTable.Add(@"EmployeeId");
			id3_NWindOrderToNewEmployee = Reader.NameTable.Add(@"NWindOrderToNewEmployee");
			id2_Item = Reader.NameTable.Add(@"");
		}
	}
	public abstract class XmlSerializer1 : System.Xml.Serialization.XmlSerializer {
		protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
			return new XmlSerializationReaderNWindOrderToNewEmployeeArray();
		}
		protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
			return new XmlSerializationWriterNWindOrderToNewEmployeeArray();
		}
	}
	public sealed class ArrayOfNWindOrderToNewEmployeeSerializer : XmlSerializer1 {
		public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
			return xmlReader.IsStartElement(@"ArrayOfNWindOrderToNewEmployee", @"");
		}
		protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
			((XmlSerializationWriterNWindOrderToNewEmployeeArray)writer).Write3_ArrayOfNWindOrderToNewEmployee(objectToSerialize);
		}
		protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
			return ((XmlSerializationReaderNWindOrderToNewEmployeeArray)reader).Read3_ArrayOfNWindOrderToNewEmployee();
		}
	}
	public class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
		public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new XmlSerializationReaderNWindOrderToNewEmployeeArray(); } }
		public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new XmlSerializationWriterNWindOrderToNewEmployeeArray(); } }
		System.Collections.Hashtable readMethods = null;
		public override System.Collections.Hashtable ReadMethods {
			get {
				if (readMethods == null) {
					System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
					_tmp[@"DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee[]::"] = @"Read3_ArrayOfNWindOrderToNewEmployee";
					if (readMethods == null) readMethods = _tmp;
				}
				return readMethods;
			}
		}
		System.Collections.Hashtable writeMethods = null;
		public override System.Collections.Hashtable WriteMethods {
			get {
				if (writeMethods == null) {
					System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
					_tmp[@"DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee[]::"] = @"Write3_ArrayOfNWindOrderToNewEmployee";
					if (writeMethods == null) writeMethods = _tmp;
				}
				return writeMethods;
			}
		}
		System.Collections.Hashtable typedSerializers = null;
		public override System.Collections.Hashtable TypedSerializers {
			get {
				if (typedSerializers == null) {
					System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
					_tmp.Add(@"DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee[]::", new ArrayOfNWindOrderToNewEmployeeSerializer());
					if (typedSerializers == null) typedSerializers = _tmp;
				}
				return typedSerializers;
			}
		}
		public override System.Boolean CanSerialize(System.Type type) {
			if (type == typeof(global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee[])) return true;
			return false;
		}
		public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
			if (type == typeof(global::DevExpress.Xpf.DemoBase.DataClasses.NWindOrderToNewEmployee[])) return new ArrayOfNWindOrderToNewEmployeeSerializer();
			return null;
		}
	}
}
namespace DevExpress.Xpf.DemoBase.Native.Serialization.GeneratedSerializers.FeedbackObject {
	public class XmlSerializationWriterFeedbackObject : System.Xml.Serialization.XmlSerializationWriter {
		public void Write4_ApprisePostDto(object o) {
			WriteStartDocument();
			if (o == null) {
				WriteNullTagLiteral(@"ApprisePostDto", @"");
				return;
			}
			TopLevelElement();
			Write3_FeedbackObject(@"ApprisePostDto", @"", ((global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.FeedbackObject)o), true, false);
		}
		void Write3_FeedbackObject(string n, string ns, global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.FeedbackObject o, bool isNullable, bool needType) {
			if ((object)o == null) {
				if (isNullable) WriteNullTagLiteral(n, ns);
				return;
			}
			if (!needType) {
				System.Type t = o.GetType();
				if (t == typeof(global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.FeedbackObject)) {
				}
				else {
					throw CreateUnknownTypeException(o);
				}
			}
			WriteStartElement(n, ns, o, false, null);
			if (needType) WriteXsiType(@"FeedbackObject", @"");
			WriteElementString(@"ModuleName", @"", ((global::System.String)o.@ModuleName));
			WriteElementString(@"Email", @"", ((global::System.String)o.@Email));
			WriteElementString(@"Feedback", @"", ((global::System.String)o.@Feedback));
			WriteElementString(@"Value", @"", Write1_RatingDto(((global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.RatingDto)o.@Value)));
			WriteElementString(@"UserId", @"", ((global::System.String)o.@UserId));
			WriteEndElement(o);
		}
		string Write1_RatingDto(global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.RatingDto v) {
			string s = null;
			switch (v) {
				case global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.RatingDto.@Negative: s = @"Negative"; break;
				case global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.RatingDto.@Neutral: s = @"Neutral"; break;
				case global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.RatingDto.@Positive: s = @"Positive"; break;
				default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.RatingDto");
			}
			return s;
		}
		protected override void InitCallbacks() {
		}
	}
	public class XmlSerializationReaderFeedbackObject : System.Xml.Serialization.XmlSerializationReader {
		public object Read4_ApprisePostDto() {
			object o = null;
			Reader.MoveToContent();
			if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
				if (((object) Reader.LocalName == (object)id1_ApprisePostDto && (object) Reader.NamespaceURI == (object)id2_Item)) {
					o = Read3_FeedbackObject(true, true);
				}
				else {
					throw CreateUnknownNodeException();
				}
			}
			else {
				UnknownNode(null, @":ApprisePostDto");
			}
			return (object)o;
		}
		global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.FeedbackObject Read3_FeedbackObject(bool isNullable, bool checkType) {
			System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
			bool isNull = false;
			if (isNullable) isNull = ReadNull();
			if (checkType) {
			if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_FeedbackObject && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
			}
			else
				throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
			}
			if (isNull) return null;
			global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.FeedbackObject o;
			o = new global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.FeedbackObject();
			bool[] paramsRead = new bool[5];
			while (Reader.MoveToNextAttribute()) {
				if (!IsXmlnsAttribute(Reader.Name)) {
					UnknownNode((object)o);
				}
			}
			Reader.MoveToElement();
			if (Reader.IsEmptyElement) {
				Reader.Skip();
				return o;
			}
			Reader.ReadStartElement();
			Reader.MoveToContent();
			int whileIterations0 = 0;
			int readerCount0 = ReaderCount;
			while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
				if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
					if (!paramsRead[0] && ((object) Reader.LocalName == (object)id4_ModuleName && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@ModuleName = Reader.ReadElementString();
						}
						paramsRead[0] = true;
					}
					else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id5_Email && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Email = Reader.ReadElementString();
						}
						paramsRead[1] = true;
					}
					else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id6_Feedback && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Feedback = Reader.ReadElementString();
						}
						paramsRead[2] = true;
					}
					else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id7_Value && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@Value = Read1_RatingDto(Reader.ReadElementString());
						}
						paramsRead[3] = true;
					}
					else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id8_UserId && (object) Reader.NamespaceURI == (object)id2_Item)) {
						{
							o.@UserId = Reader.ReadElementString();
						}
						paramsRead[4] = true;
					}
					else {
						UnknownNode((object)o, @":ModuleName, :Email, :Feedback, :Value, :UserId");
					}
				}
				else {
					UnknownNode((object)o, @":ModuleName, :Email, :Feedback, :Value, :UserId");
				}
				Reader.MoveToContent();
				CheckReaderCount(ref whileIterations0, ref readerCount0);
			}
			ReadEndElement();
			return o;
		}
		global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.RatingDto Read1_RatingDto(string s) {
			switch (s) {
				case @"Negative": return global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.RatingDto.@Negative;
				case @"Neutral": return global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.RatingDto.@Neutral;
				case @"Positive": return global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.RatingDto.@Positive;
				default: throw CreateUnknownConstantException(s, typeof(global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.RatingDto));
			}
		}
		protected override void InitCallbacks() {
		}
		string id1_ApprisePostDto;
		string id3_FeedbackObject;
		string id4_ModuleName;
		string id7_Value;
		string id5_Email;
		string id6_Feedback;
		string id8_UserId;
		string id2_Item;
		protected override void InitIDs() {
			id1_ApprisePostDto = Reader.NameTable.Add(@"ApprisePostDto");
			id3_FeedbackObject = Reader.NameTable.Add(@"FeedbackObject");
			id4_ModuleName = Reader.NameTable.Add(@"ModuleName");
			id7_Value = Reader.NameTable.Add(@"Value");
			id5_Email = Reader.NameTable.Add(@"Email");
			id6_Feedback = Reader.NameTable.Add(@"Feedback");
			id8_UserId = Reader.NameTable.Add(@"UserId");
			id2_Item = Reader.NameTable.Add(@"");
		}
	}
	public abstract class XmlSerializer1 : System.Xml.Serialization.XmlSerializer {
		protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
			return new XmlSerializationReaderFeedbackObject();
		}
		protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
			return new XmlSerializationWriterFeedbackObject();
		}
	}
	public sealed class FeedbackObjectSerializer : XmlSerializer1 {
		public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
			return xmlReader.IsStartElement(@"ApprisePostDto", @"");
		}
		protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
			((XmlSerializationWriterFeedbackObject)writer).Write4_ApprisePostDto(objectToSerialize);
		}
		protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
			return ((XmlSerializationReaderFeedbackObject)reader).Read4_ApprisePostDto();
		}
	}
	public class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
		public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new XmlSerializationReaderFeedbackObject(); } }
		public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new XmlSerializationWriterFeedbackObject(); } }
		System.Collections.Hashtable readMethods = null;
		public override System.Collections.Hashtable ReadMethods {
			get {
				if (readMethods == null) {
					System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
					_tmp[@"DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper+FeedbackObject::ApprisePostDto:True:"] = @"Read4_ApprisePostDto";
					if (readMethods == null) readMethods = _tmp;
				}
				return readMethods;
			}
		}
		System.Collections.Hashtable writeMethods = null;
		public override System.Collections.Hashtable WriteMethods {
			get {
				if (writeMethods == null) {
					System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
					_tmp[@"DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper+FeedbackObject::ApprisePostDto:True:"] = @"Write4_ApprisePostDto";
					if (writeMethods == null) writeMethods = _tmp;
				}
				return writeMethods;
			}
		}
		System.Collections.Hashtable typedSerializers = null;
		public override System.Collections.Hashtable TypedSerializers {
			get {
				if (typedSerializers == null) {
					System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
					_tmp.Add(@"DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper+FeedbackObject::ApprisePostDto:True:", new FeedbackObjectSerializer());
					if (typedSerializers == null) typedSerializers = _tmp;
				}
				return typedSerializers;
			}
		}
		public override System.Boolean CanSerialize(System.Type type) {
			if (type == typeof(global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.FeedbackObject)) return true;
			return false;
		}
		public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
			if (type == typeof(global::DevExpress.Xpf.DemoBase.Helpers.FeedbackHelper.FeedbackObject)) return new FeedbackObjectSerializer();
			return null;
		}
	}
}
#pragma warning restore DX0008
