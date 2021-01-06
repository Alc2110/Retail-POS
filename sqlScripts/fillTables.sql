--Staff
INSERT INTO Staff (FullName, PasswordHash, Privelege)
VALUES ('user a', '1262301944193641595511921623780187391981092451687821318915315425151391968922796136', 'Admin');

INSERT INTO Staff (FullName, PasswordHash, Privelege)
VALUES ('user n', '1771215959170212241201912419720863234153588222241158114225160147450620532073745', 'Normal');

--Customers
INSERT INTO Customers (FullName, StreetAddress, PhoneNumber, Email, City, State_, Postcode)
VALUES ('John Smith', '1 Example St', '123456', 'johnsmith@email.com', 'Somewhere', 'State', 0000);

INSERT INTO Customers (FullName, StreetAddress, PhoneNumber, Email, City, State_, Postcode)
VALUES ('Steve Simpson', '12 Demonstration Cr', '654321', 'stevesimpson@email.com', 'Elsewhere', 'State', 0000);

--Products
INSERT INTO Products (ProductIDNumber, Description_, Quantity, Price)
VALUES ('9398830003299', 'Mens Deodorant', 100, 4.95);

INSERT INTO Products(ProductIDNumber, Description_, Quantity, Price)
VALUES ('9311059007561', 'Hair Gel', 50, 50);

INSERT INTO Products(ProductIDNumber, Description_, Quantity, Price)
VALUES ('1324793501001', 'Binoculars', 20, 15.95);

INSERT INTO Products(ProductIDNumber, Description_, Quantity, Price)
VALUES ('0041491987655', 'Programming Book', 25, 60);

INSERT INTO Products(ProductIDNumber, Description_, Quantity, Price)
VALUES ('66782502134', 'Headphones', 25, 25.95);
