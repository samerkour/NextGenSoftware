#### Migration Scripts

```bash
dotnet ef migrations add InitialNotificationsMigration -o Shared/Data/Migrations/Notification -c NotificationsDbContext
dotnet ef database update -c NotificationsDbContext
```
```using PMC
Add-Migration -Name InitialNotificationsMigration -o Shared/Data/Migrations/Notification -c NotificationsDbContext
Update-Database -context NotificationsDbContext
Remove-Migration -context NotificationsDbContext
```
