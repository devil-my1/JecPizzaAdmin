Select purDate, SUM(Total) as total from Purchase where PurDate like '202-_11%'  group by PurDate

select * from Purchase
select * from Cart
select * from Goods
select * from Member
 
Select temp.Username, Sum(temp.total) as sougoKaikei from (select M.UserName,G.Name,(Select GoodsGroup.GroupName  from GoodsGroup where GoodsGroupId = g.GoodsGroupId) as GroupType,(Quantity * G.Price) as total from Cart
join Goods as G on Cart.GoodsId = G.GoodsId
join Member as M on M.MemberId = Cart.MemberId) as temp group by temp.UserName


Select  GG.GroupName, SUM(Quantity) as Quantity from Cart join  Goods as G on G.GoodsId = Cart.GoodsId join GoodsGroup as GG on GG.GoodsGroupId = G.GoodsGroupId where CartId = 'C112' group by GG.GroupName

Select CartId, MemberId from Purchase where PurDate = '2022-01-10'

select SUM(Quantity*G.Price) as total from Cart join Goods as G on G.GoodsId = Cart.GoodsId where CartId='C788'

insert into Purchase values
('P7681','C788','202112079565',Null,'4263982532254299','2022-01-12',16000)

delete from Purchase where PurchaseId = 'P7681'

Insert into Member Values
('202112079561','Lil X2','Liya Nex','li.li12@gmail.com','150-1149','東京都新宿区殿平350ツフランス103-F','641wrfhb','1992-01-26','F','081-4913-4448',NULL,NULL,Getdate());

insert into Cart Values
('C788','202112079565','7944',1),
('C788','202112079565','4956',3),
('C788','202112079565','6584',1),
('C788','202112079565','5869',2),
('C112','202112079567','4896',3),
('C112','202112079567','4864',2),
('C424','202112079561','7944',3),
('C424','202112079561','7895',2),
('C424','202112079561','4956',2)

update Purchase set Total = 15700 where MemberId = '202112079561' and PurDate = '2022-01-10'