﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MisaCommon.Utility {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class CommonMessage {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CommonMessage() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MisaCommon.Utility.CommonMessage", typeof(CommonMessage).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   値を空にすることはできません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ArgumentExceptionMessageEmpty {
            get {
                return ResourceManager.GetString("ArgumentExceptionMessageEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   この引数は {0} 未満の値にできません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ArgumentOutOfRangeExceptionLessThan {
            get {
                return ResourceManager.GetString("ArgumentOutOfRangeExceptionLessThan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   この引数は {0} を超えた値にできません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ArgumentOutOfRangeExceptionMoreThan {
            get {
                return ResourceManager.GetString("ArgumentOutOfRangeExceptionMoreThan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   この引数は {0} 以下の値にできません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ArgumentOutOfRangeExceptionOrLess {
            get {
                return ResourceManager.GetString("ArgumentOutOfRangeExceptionOrLess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   この引数は {0} 以上の値にできません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ArgumentOutOfRangeExceptionOrMore {
            get {
                return ResourceManager.GetString("ArgumentOutOfRangeExceptionOrMore", resourceCulture);
            }
        }
    }
}
