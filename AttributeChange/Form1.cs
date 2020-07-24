using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttributeChange {
    public partial class Form1 : Form {
        private MyClass obj = new MyClass();
        private AttributeOption option = new AttributeOption();
        public Form1() {
            InitializeComponent();
            propertyGrid1.SelectedObject = obj;
            propertyGrid2.SelectedObject = option;
        }

        private void propertyGrid2_PropertyValueChanged(object s, PropertyValueChangedEventArgs e) {
            SetPropertyAttributeReadOnly(typeof(MyClass), "Item", option.ReadOnly);
            SetPropertyAttributeDisplayName(typeof(MyClass), "Item", option.DisplayName);
            propertyGrid1.Refresh();
        }
        
        public static void SetPropertyAttributeReadOnly(Type type, string propertyName, bool value) {
            SetPropertyAttributeField(type, propertyName, typeof(ReadOnlyAttribute), "isReadOnly", value);
        }

        public static void SetPropertyAttributeDisplayName(Type type, string propertyName, string value) {
            SetPropertyAttributeField(type, propertyName, typeof(DisplayNameAttribute), "_displayName", value);
        }

        public static void SetPropertyAttributeField(Type type, string propertyName, Type attributeType, string attributeFieldName, object attributeFieldValue) {
            PropertyDescriptor typeProp = TypeDescriptor.GetProperties(type)[propertyName];   // 타입과 이름으로 프로퍼티 구함
            var attribute = typeProp.Attributes[attributeType];                               // 프로퍼티와 어트리뷰트타입으로 프로퍼티의 어트리뷰트 구함
            var attrField = attribute.GetType().GetField(attributeFieldName, BindingFlags.NonPublic | BindingFlags.Instance);  // 필드이름으로 필드 구함
            attrField.SetValue(attribute, attributeFieldValue);     // 필드 값 변경
        }
    }

    public class MyClass {
        [DisplayName("이름")]
        [ReadOnly(false)]
        public int Item { get; set; } = 0;
    }

    public class AttributeOption {
        public string DisplayName { get; set; } = "이름";
        public bool ReadOnly { get; set; } = false;
    }
}
