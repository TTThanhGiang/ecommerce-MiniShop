# Website Bán Hàng MiniShop

## Mô tả

Dự án này là một website bán hàng đơn giản sử dụng ASP.NET MVC và Entity Framework (Database First). Website cung cấp các chức năng như xem sản phẩm, bài đăng, quản lý bài đăng,
quản lý giỏ hàng, đặt hàng, và quản lý sản phẩm.

## Yêu cầu hệ thống

- **Hệ điều hành**: Windows 10 hoặc cao hơn
- **Phần mềm**:
  - Visual Studio 2019
  - .NET Framework 4.7.2 hoặc cao hơn
  - SQL Server 2019 hoặc cao hơn

## Hướng dẫn cài đặt
### 1. Clone repository
Clone repository từ GitHub:
git clone https://github.com/TTThanhGiang/ThanhGiang_WebCuoiKi.git

### 2. Mở project 
Mở file trong project với đuôi .sln

### 3. Khôi phục các gói NuGet
Mở NuGet Package Manager Console từ Tools -> NuGet Package Manager -> Package Manager Console.
Chạy lệnh sau:
Update-Package -reinstall

### 4. Đảm bảo đã chạy file sql
Chạy phần create table trước rồi add dữ liệu sau

### 5. Cấu hình cơ sở dữ liệu
Mở Web.config và chỉnh sửa chuỗi kết nối: Thay source=LAPTOP-9TOR6RJG thành server cá nhân
<connectionStrings>
    <add name="dbMiniShopEntities2" connectionString="metadata=res://*/Models.MiniShopModel.csdl|res://*/Models.MiniShopModel.ssdl|res://*/Models.MiniShopModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=LAPTOP-9TOR6RJG;initial catalog=dbMiniShop;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
</connectionStrings>
### 6. Run project
Tk Admin: thanhgiang
      mk: 123456

Tk Customer: vanan
      mk: 123456
