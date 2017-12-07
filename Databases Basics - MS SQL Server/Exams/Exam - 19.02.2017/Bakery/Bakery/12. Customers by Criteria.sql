SELECT cu.FirstName, cu.Age, cu.PhoneNumber
FROM Customers AS cu
INNER JOIN Countries AS co ON co.Id = cu.CountryId
WHERE (Age >= 21 AND FirstName LIKE '%an%') 
OR ((RIGHT(PhoneNumber, 2) = '38') AND co.Name <> 'Greece')
ORDER BY FirstName,
         Age DESC