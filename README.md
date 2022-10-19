# TwiColle
推特圖片保存與檢視的WebAPI
使用ASP.NET WebAPI with Entity Framework、MSSQL

API功能
  1.利用瀏覽器右鍵腳本取得當前推特的資訊(網址、時間、圖片連結等)加入自定義Tag後，以POST方法呼叫API保存至資料庫
  2.GET取得圖片
    (1)AllPhoto             ~/api/Photo
    (2)SearchByArtist       ~/api/Artist/{name}
    (3)SearchByTag          ~/api/Tag/{name}
  3.刪除圖片
  4.Tag的新增與刪除
  5.Artist的變更(推特允許變更帳號名稱)
  
PhotoController
~/api/Photo
[GET]
[POST]
[DELETE]

ArtistController
~/api/Artist
[GET]
[PUT]

TagController
~/api/Tag
[GET]
[POST]
[DELETE]

