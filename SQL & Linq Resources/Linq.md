## Core SQL Clauses as Linq

Here are the standard clauses you will use in almost every query, listed in the order they are typically written:
### `SELECT`, `FROM`, `WHERE`
#### SQL 
`SELECT`: Specifies which columns you 
- **Example:** `SELECT first_name, email`
`FROM`: Specifies which table (or tables, using `JOIN`) to pull the data from.
- **Example:** `FROM customers`
`WHERE`: Filters rows based on a specific condition _before_ any grouping happens.
- **Example:** `WHERE country = 'Canada'`

#### LINQ

```
// Query Syntax
var queryResult = from customer in customers
                  where customer.Country == "Canada"
                  select new 
                  { 
                      customer.FirstName, 
                      customer.Email 
                  };
```

```
// Method Syntax
var methodResult = customers
    .Where(customer => customer.Country == "Canada")
    .Select(customer => new 
    { 
        customer.FirstName, 
        customer.Email 
    });
```

### `GROUP BY`, `HAVING`, `ORDER BY`, `TOP`
#### SQL
`GROUP BY`: Groups rows that have the same values into summary rows. It is almost always used with aggregate functions like `COUNT()`, `SUM()`, or `AVG()`.
- **Example:** `GROUP BY country`
`HAVING`:  groups created by the `GROUP BY` clause. (Note: `WHERE` filters individual rows; `HAVING` filters aggregated groups).
- **Example:** `HAVING COUNT(customer_id) > 100`
`ORDER BY`: Sorts the final result set in ascending (`ASC`) or descending (`DESC`) order.
- **Example:** `ORDER BY last_name DESC`
`TOP`: Restricts the total number of rows returned. 
- **Example:** `TOP 10`
```
SELECT country, COUNT(customer_id) AS total_customers
FROM customers
WHERE status = 'Active'
GROUP BY country
HAVING COUNT(customer_id) > 5
ORDER BY total_customers DESC
TOP 5;
```
#### LINQ
```
var queryResult = (from c in customers
                   where c.Status == "Active"
                   group c by c.Country into grouped
                   let totalCustomers = grouped.Count()
                   where totalCustomers > 5
                   orderby totalCustomers descending
                   select new 
                   { 
                       Country = grouped.Key, 
                       TotalCustomers = totalCustomers 
                   })
                  .Take(5);
```

```
var methodResult = customers
    .Where(c => c.Status == "Active")
    .GroupBy(c => c.Country)
    .Select(g => new 
    { 
        Country = g.Key, 
        TotalCustomers = g.Count() 
    })
    .Where(x => x.TotalCustomers > 5)
    .OrderByDescending(x => x.TotalCustomers)
    .Take(5);
```


### Built In Functions 

#### CAST
SQL: `CAST(expression AS target_data_type)`
Linq:
- Use C# casting operators `(type)`, conversion methods like `Convert.ToInt32()`, or parsing methods like `int.Parse()`.
- Cast an int as a double: `customers.Select(c => (double)c.AgeInt);`
- Cast a string as an int: `customers.Select(c => int.Parse(c.ZipCodeString));`
#### SUBSTRING
SQL: `SUBSTRING(string, start_position, length)`
Linq:
- Use the C# `.Substring(startIndex, length)` method. Note that C# strings are 0-indexed, whereas SQL is 1-indexed.
- `customers.Select(c => c.Name.Substring(0, 3));`
#### LEN
SQL: `LEN(string)`
Linq:
- Access the `.Length` property of the string.
- `customers.Select(c => c.Name.Length);`
#### TRIM
SQL: `TRIM(string)`
Linq:
- Use the `.Trim()` method to clear leading and trailing whitespace.
- `customers.Select(c => c.Name.Trim());`
#### CONCAT
SQL: `CONCAT(string1, string2, ...)`
LinqL
- Use C# string interpolation (`$"{var1} {var2}"`) or `string.Concat()`.
- `customers.Select(c => $"{c.FirstName} {c.LastName}");`
#### COUNT
SQL: ``COUNT(column_name)`` or ``COUNT(*)``
Linq:
- Use the `.Count()` extension method, either on the whole collection or passing a filtering lambda.
- `customers.Count(c => c.IsActive);`
#### SUM
SQL: `SUM(column_name)`
Linq:
- Use the `.Sum()` extension method with a lambda targeting the numeric property.
- `orders.Sum(o => o.TotalAmount);`
#### AVG
SQL: `AVG(column_name)`
Linq:
- Use the `.Average()` extension method with a lambda targeting the numeric property.
- `orders.Average(o => o.TotalAmount);`
#### MIN/Max
SQL: `MIN` or (`MAX`) 
Linq:
- Use the `.Min()` or `.Max()` extension methods with a lambda targeting the property.
- `var lowest = orders.Min(o => o.Amount);`
- `var highest = orders.Max(o => o.Amount);`
#### COALESCE
SQL: `COALESCE(value1, value2, ..., fallback_value)`
Linq:
- Use the C# null-coalescing operator `??` to return the first non-null value in the sequence.
- `customers.Select(c => c.PhoneNumber ?? c.Email ?? "No Contact Info");`

### SQL Comparison in Linq
`AND`
- Combine conditions using the logical AND operator `&&`.
- Linq:`customers.Where(c => c.Age >= 18 && c.IsActive);`
`OR`
- Combine conditions using the logical OR operator `||`.
- Linq: `customers.Where(c => c.Country == "Canada" || c.Country == "US");`
`NOT`
- Negate a boolean expression using the logical negation operator `!`.
- Linq: `customers.Where(c => !c.IsActive);`
`=`
- Use the standard C# equality operator `==`.
- Linq: `customers.Where(c => c.Country == "Canada");`
`<>` 
- Use the standard C# inequality operator `!=`.
- Linq: `customers.Where(c => c.Country != "Canada");`
`>,<, >=, <=`
- Use standard C# comparison operators.
- Linq: `customers.Where(c => c.Age > 18);` or `customers.Where(c => c.Age < 65);`
BETWEEN	
- SQL: `WHERE age BETWEEN 18 AND 25`
- Linq: customers.Where(c => c.Age >= 18 && c.Age <= 25);
IN	
- `WHERE country IN ('UK', 'US', 'CA')`
Linq:  
  ```
var targetCountries = new[] { "UK", "US", "CA" };
var result = customers.Where(c => targetCountries.Contains(c.Country));
  ```

LIKE	
- SQL: `WHERE name LIKE 'J%'`
- Map `%` wildcards to `.StartsWith()`, `.EndsWith()`, or `.Contains()`. For single-character wildcards (`_`), use C# pattern matching or regular expressions. If using Entity Framework Core, use `EF.Functions.Like()`.
Linq:
```
// 'J%' (Starts with J)
customers.Where(c => c.Name.StartsWith("J"));

// '%J' (Ends with J)
customers.Where(c => c.Name.EndsWith("J"));

// '%J%' (Contains J)
customers.Where(c => c.Name.Contains("J"));

// 'J_y' (Single character wildcard using EF Core)
customers.Where(c => EF.Functions.Like(c.Name, "J_y"));
```

IS NULL	
- SQL: `WHERE phone_number IS NULL`
- Linq: `customers.Where(c => c.PhoneNumber == null);`
EXISTS
SQL:
```
SELECT department_name 
FROM departments d 
WHERE EXISTS ( 
	SELECT 1 
	FROM employees e 
	WHERE e.department_id = d.department_id 
);
```
Linq:
```
// Standalone check
bool hasActiveCustomers = customers.Any(c => c.IsActive);

// Subquery check (Find orders where a customer exists)
var ordersWithCustomers = orders.Where(o => customers.Any(c => c.Id == o.CustomerId));
```

JOIN
SQL:
```
-- Returns only employees who are assigned to an existing department
SELECT 
    Employees.FirstName, 
    Employees.LastName, 
    Departments.DepartmentName
FROM Employees
INNER JOIN Departments 
    ON Employees.DepartmentID = Departments.DepartmentID;
```

Linq:
```
-- Returns only employees who are assigned to an existing department
SELECT 
    Employees.FirstName, 
    Employees.LastName, 
    Departments.DepartmentName
FROM Employees
INNER JOIN Departments 
    ON Employees.DepartmentID = Departments.DepartmentID;
```

```
var innerJoinResult = Employees.Join(
    Departments,
    employee => employee.DepartmentID,
    department => department.DepartmentID,
    (employee, department) => new
    {
        employee.FirstName,
        employee.LastName,
        department.DepartmentName
    }
);
```

