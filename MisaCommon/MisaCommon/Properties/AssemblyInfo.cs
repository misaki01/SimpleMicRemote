using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// アセンブリに関する一般情報は以下の属性セットをとおして制御されます。
// アセンブリに関連付けられている情報を変更するには、
// これらの属性値を変更してください。
[assembly: AssemblyTitle("共通ライブラリ")]
[assembly: AssemblyDescription("共通ライブラリ")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("みさきさん")]
[assembly: AssemblyProduct("共通ライブラリ")]
[assembly: AssemblyCopyright("Copyright © 2019 みさきさん. All Rights Reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// アゼンブリがCLSに準拠している場合は true 、準拠していない場合は false を設定してください。
// （「Microsoftのすべての規則」の条件でコード分析を行いエラーをなくしているため、
// 　　CLSに準拠していると思いたい。多分おそらく・・・、
// 　　よくわからないけど、コード分析で検出されるかもしれないから一応 true にしておく）
[assembly: CLSCompliant(true)]

// ComVisible を false に設定すると、このアセンブリ内の型は COM コンポーネントから
// 参照できなくなります。COM からこのアセンブリ内の型にアクセスする必要がある場合は、
// その型の ComVisible 属性を true に設定してください。
[assembly: ComVisible(false)]

// このプロジェクトが COM に公開される場合、次の GUID が typelib の ID になります
[assembly: Guid("ffb01105-5f72-41df-97a9-b8fd513f1cb2")]

// アセンブリのバージョン情報は、以下の 4 つの値で構成されています:
//
//      メジャー バージョン
//      マイナー バージョン
//      ビルド番号
//      リビジョン
//
// すべての値を指定するか、次を使用してビルド番号とリビジョン番号を既定に設定できます
// 既定値にすることができます:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.6.0.0")]
[assembly: AssemblyFileVersion("0.6.0.0")]
[assembly: NeutralResourcesLanguage("ja")]
