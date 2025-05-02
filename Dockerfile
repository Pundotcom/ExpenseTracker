# ใช้ภาพพื้นฐานจาก ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 10000  # ใช้พอร์ตที่ Render กำหนด (10000)

# ใช้ SDK ของ .NET สำหรับการสร้างโปรเจค
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .  # คัดลอกไฟล์จากโฟลเดอร์โปรเจค
RUN dotnet restore "ExpenseTracker.csproj"  # ดาวน์โหลด package dependencies
RUN dotnet build "ExpenseTracker.csproj" -c Release -o /app/build  # สร้างโปรเจค

# สร้างไฟล์ที่พร้อมใช้งาน
FROM build AS publish
RUN dotnet publish "ExpenseTracker.csproj" -c Release -o /app/publish  # สร้างไฟล์ที่พร้อมใช้งาน

# ใช้ไฟล์ที่ได้จากการ publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .  # คัดลอกไฟล์ที่ถูก publish จาก build stage มาใช้งาน
ENTRYPOINT ["dotnet", "ExpenseTracker.dll"]  # เรียกใช้แอปพลิเคชัน

