select * from Cart
select * from Card
select * from Member
select * from GoodsGroup
Select * from Goods
Select * from Reservation
Select * from Delivery
Select * from Purchase



insert into GoodsGroup values
('CH','Cheese'),
('PA','Pasta'),
('DE','Desert'),
('DR','Drinks'),
('SA','Salad'),
('SI','Side')


insert into Goods Values
('0256','Bismark',1250,'PI','18.jpg',0,0,1),
('1569','Natale',1550,'PI','3.jpg',1,1,1),
('0984','Pizza Bratta',1950,'PI','6.jpg',1,0,1),
('4864','Alpino',2250,'PI','19.jpg',1,0,1),
('7746','Brutta',1150,'CH','01.jpg',1,0,1),
('4896','Vilage Potetos',900,'SI','11.jpg',0,1,1),
('7441','Margarita Ahijo',1520,'SI','14.jpg',1,1,1),
('4789','WheyStrawbary',600,'DR','Dr1.jpg',1,1,1)

Insert into Reservation values
('R498','080-4441-0581','2022-1-05 18:30',3,'5',1),
('R489','080-5556-4513','2022-1-08 19:00',2,'9',1)

Insert into Delivery values
('D441','202112079564','千葉県松戸市殿平賀390ハイツフレグランス','2022-01-6')

Insert into Purchase values
('P512','C489','202112079564',Null,'4263982640269299',GETDATE(),4500)