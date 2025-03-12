# Azure Storage Queue 示範專案

這個專案展示了如何使用 Azure Storage Queue 進行基本的訊息佇列操作。

## 功能說明

本專案實現以下功能：
1. 建立訊息佇列
2. 插入訊息至佇列
3. 查看佇列中的訊息
4. 處理並移除佇列中的訊息

## 主要元件

- `CreateQueueClient`: 建立佇列客戶端
- `CreateQueue`: 建立新的訊息佇列
- `InsertMessage`: 將訊息插入佇列
- `PeekNextMessage`: 預覽下一個佇列訊息
- `DequeueMessage`: 處理並移除佇列訊息

## 程式碼函式說明

### Main 方法
主程式進入點，執行以下操作：
- 建立以日期時間命名的佇列
- 插入 10 條測試訊息
- 等待使用者輸入後讀取訊息
- 持續讀取直到佇列清空

### CreateQueueClient 方法
```csharp
public static void CreateQueueClient(string queueName)
```
建立佇列客戶端的方法：
- 從設定檔讀取連接字串
- 建立 QueueClient 實例以操作佇列
- 參數 queueName：佇列名稱

### CreateQueue 方法
```csharp
public static bool CreateQueue(string queueName)
```
建立新佇列的方法：
- 使用連接字串建立 QueueClient
- 如果佇列不存在則建立
- 回傳佇列建立狀態（布林值）
- 包含異常處理機制

### InsertMessage 方法
```csharp
public static void InsertMessage(string queueName, string message)
```
插入訊息到佇列：
- 連接指定的佇列
- 確保佇列存在
- 發送訊息到佇列
- 參數：
  - queueName：佇列名稱
  - message：要發送的訊息內容

### PeekNextMessage 方法
```csharp
public static bool PeekNextMessage(string queueName)
```
預覽佇列中的下一則訊息：
- 不會移除訊息
- 確認佇列中是否還有訊息
- 回傳是否存在下一則訊息

### DequeueMessage 方法
```csharp
public static string DequeueMessage(string queueName)
```
處理並移除佇列中的訊息：
- 讀取下一則訊息
- 處理訊息內容
- 從佇列中刪除該訊息
- 回傳訊息內容字串

## 使用方式

1. 確保已設定 Azure Storage 連接字串在設定檔中：
   ```xml
   <appSettings>
     <add key="StorageConnectionString" value="DefaultEndpointsProtocol=https;AccountName=____________;AccountKey=v7MyKrNXMJaDQLJe6AAMiW+AHkA/Z7qp_________Yu2jQgkdISgODQ2lrNcE4b+ASteiZSDw==;EndpointSuffix=core.windows.net"/>
   </appSettings>
   ```

2. 執行程式後會：
   - 自動建立一個以日期命名的佇列
   - 插入 10 條測試訊息
   - 等待使用者按鍵後開始讀取訊息
   - 顯示並移除所有訊息

## 注意事項

- 需要確保 Azurite 儲存體模擬器正在運行
- 佇列名稱格式為 "myqueue-yyyyMMddHH"
- 訊息處理時間應少於 30 秒

## 開發環境需求

- .NET Core 3.1 或更新版本
- Azure.Storage.Queues NuGet 套件
- Azure.Identity NuGet 套件
- System.Configuration.ConfigurationManager NuGet 套件
