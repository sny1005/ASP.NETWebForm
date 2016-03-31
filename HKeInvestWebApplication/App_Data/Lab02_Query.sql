SELECT lastName, firstName FROM Client WHERE lastName LIKE 'La%';

SELECT accountNumber FROM Account WHERE balance = 0;

SELECT DISTINCT accountNumber FROM SecurityHolding WHERE code = 22;

SELECT accountNumber FROM Account EXCEPT 
SELECT DISTINCT accountNumber FROM SecurityHolding;

SELECT accountType, COUNT(accountType) FROM Account GROUP BY accountType;

SELECT firstName, lastName, email, HKIDPassportNumber FROM Client
WHERE accountNumber IN (
	SELECT accountNumber FROM SecurityHolding WHERE code = 22 AND type = 'stock'
	)
ORDER BY lastName;