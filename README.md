# Game_Framework
一、LatestHotFixGameFrame(Version_2024)

最新个人热更游戏框架

1、使用Phpstudy_pro搭建本地服务器；

2、Untiy主工程：FrameStudy

（1）使用Unity2021.3.39f、VS2022版本开发；

（2）通过UnityPackage里获取最新ILRuntime添加到项目；

3、VS创建的HotFix热更工程：FrameStudy/HotFixProject

（1）根据ILRuntime官方文档说明，使用.Net Framework 4.6框架；

（2）适配.Net Framework 4.6框架，VS获取2.4.0版本Protobuf-net使用;

4、说明：

（1）可参考Analysis_Project文件夹里文档进行项目学习使用；

（2）项目基于BasicGameFrame基础上于2024-8月实现并测试完成。


二、HotFixGameFrame(Version_2019)

早版本的游戏资源代码热更框架

1、热更工程使用.Net Framework 4.6框架；

2、对应通过VS获取使用的Protobuf-net是2.4.0版本;

3、通过ILRuntimeDemo迁移到项目，获取使用ILRuntime插件；

4、还包含资源AES标准加密，ABMD5文件校验、资源解压适配Andriod、

5、自动打标准包、热更包、更新hotFix.dll、Protobuf序列化等功能；

5、基于BasicGameFrame基础上于2019年开发完成；


三、BasicGameFrame(Version_2018)

游戏底层资源加载框架

1、实现底层对资源的加载释放等管理：

2、包含自定义ab包打包策略、ab包依赖加载，类对象池，资源池，

3、对象池，离线数据，xml、 Binary、 Excel转换等功能；

4、于2018年开发实现，可正常使用。