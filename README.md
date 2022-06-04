# PTSoft.BDSManager
自动获取插件在registerPlugin时设置的git或者github地址，并获取最新release，如果存在较新的版本，则在控制台打印更新提醒，同时自动下载最新版到<服务器>/downloads目录下。

### 如何接入自动检测？
只需在注册插件的时候提供github地址即可纳入检测范围。本插件会每两小时向github发送一次http get请求，以获取最新release，判定标准为tag中包含的版本信息，所以请务必在release的tag内至少包含一段格式为`major.minor.reversion`的完整的版本信息，否则将无法被识别。

registerPlugin示例：
```javascript
// JavaScript
// 以JS为例，lua和C++亦然
const META_DATA = {
    Git:'https://github.com/xxx/xxx/',
    SomethingOther:'...',
    ...
};
ll.registerPlugin('ExamplePlugin', 'introduction', [1, 9, 19810], META_DATA);
```
