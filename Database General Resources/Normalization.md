To understand normalization, we have to look closely at **redundancy**. In database design, redundancy isn't just "having a lot of data"—it is specifically the **unnecessary repetition of data resulting from a poor table design**.

When the same piece of information is stored in multiple places, it opens the door to three specific data anomalies:
- **Insertion Anomaly:** You cannot store certain data because it requires other, unrelated data to exist first.
- **Update Anomaly:** To update a single real-world fact, you have to modify multiple rows. If you miss one, your data becomes inconsistent.
- **Deletion Anomaly:** Deleting a record accidentally wipes out unrelated, valuable information.

## Functional Dependencies
At its core, a **functional dependency** in a database is just a relationship between columns where the value of one column completely determines the value of another.
Think of it like a function in math or programming: if you pass in input $X$, you will always get exactly one output $Y$. We write this relationship as:

$$X \rightarrow Y$$
This reads as **"X functionally determines Y"** or **"Y is dependent on X."**
#### A Real-World Analogy
Imagine a spreadsheet tracking employees:
- **Employee ID $\rightarrow$ Employee Name:** If I give you the Employee ID `E001`, you look at the table and tell me the name is "Dave." If you look up `E001` tomorrow, it will still be "Dave." The ID completely determines the name.
- **Employee Name $\not\rightarrow$ Employee ID:** If I give you the name "John Smith," you can't confidently give me a single ID because there might be three different John Smiths working at the company. Name does _not_ functionally determine ID.
### The Formal Rule
If two rows in a table have the exact same value in column $X$, they **must** have the exact same value in column $Y$. Look at this basic `Users` table:

| **UserID (X)** | **Email (Y)**   | **City** |
| -------------- | --------------- | -------- |
| 101            | dave@email.com  | Winnipeg |
| 102            | sarah@email.com | Toronto  |
| 101            | dave@email.com  | Winnipeg |

Because `UserID` `101` always maps to `dave@email.com`, **UserID $\rightarrow$ Email** holds true.
### Why Do We Care in Database Design?
Functional dependencies are the foundational building blocks for **normalization** (organizing a database to reduce data redundancy and prevent bugs).
They help us find two major things:
1. **Candidate Keys:** If a single column (or a mix of columns) functionally determines _every other column_ in the table, that column can serve as your Primary Key.
2. **Bad Groupings (Redundancy):** If column $A$ determines column $B$, but column $A$ isn't the primary key, you usually have a design flaw.

## Keys (refresher)
### Superkey (The Broadest Category)

A **Superkey** is any column—or any combination of columns—that can uniquely identify a row in a table.

It doesn't care about efficiency. If adding extra columns to a unique identifier still keeps it unique, it’s still a superkey.

- **Analogy:** If I want to find you in a crowded room, knowing your _Social Insurance Number_ is enough. Knowing your _Social Insurance Number + Your Favorite Color + Your Height_ also works. That giant combo is a superkey.
    
- **Example:** In an `Employees` table, `(EmployeeID)` is a superkey. But `(EmployeeID, Email)` and `(EmployeeID, FavoriteFood, City)` are also superkeys.
    

### Candidate Key (The Minimalist Superkey)
A **Candidate Key** is a superkey that has absolutely no fat left to trim. If you remove even one column from it, it loses its ability to uniquely identify the row. It is a **minimal superkey**.
A table can have multiple candidate keys. They are the "candidates" running for the job of Primary Key.
- **Example:** In a `Users` table, both `EmployeeID` and `Email` are unique on their own. If you look at the combined key `(EmployeeID, Email)`, it is a superkey, but _not_ a candidate key because you could easily drop `Email` and still uniquely identify everyone. `EmployeeID` by itself is a candidate key; `Email` by itself is another.


### Primary Key (The Chosen One)
The **Primary Key** is simply the specific candidate key that you, the database designer, pick to be the official, main identifier for the table.
All other candidate keys that didn't get picked are called **Alternate Keys**.
## Normalizing a Table
### 1. Starting at 1NF (First Normal Form)

A table is in 1NF if all attributes contain atomic (indivisible) values, and there are no repeating groups.

Let's look at a **CohortAssignments** table that tracks students, the cohorts they are in, the main instructor for that cohort, and the office location of that instructor.

#### The 1NF Table

| **StudentID** | **StudentName** | **CohortID** | **Instructor** | **InstructorOffice** |
| ------------- | --------------- | ------------ | -------------- | -------------------- |
| **101**       | Alice Smith     | COMP-101     | Jane Doe       | Room 302             |
| **102**       | Bob Jones       | COMP-101     | Jane Doe       | Room 302             |
| **103**       | Charlie Brown   | WD-202       | John Maxwell   | Room 415             |
| **104**       | Diana Prince    | COMP-101     | Jane Doe       | Room 302             |

- **Primary Key:** `(StudentID, CohortID)`
- **Functional Dependencies (FDs):**
    1. `StudentID` $\rightarrow$ `StudentName`
    2. `CohortID` $\rightarrow$ `Instructor`
    3. `Instructor` $\rightarrow$ `InstructorOffice`

#### The Redundancy and Anomalies here:
Look at rows 1, 2, and 4. The fact that `COMP-101` is taught by `Jane Doe` in `Room 302` is repeated three separate times.
- **Update Anomaly:** If Jane Doe moves to Room 501, we have to update three rows. If we miss Bob's row, the database is broken.
- **Insertion Anomaly:** We cannot create a new cohort (e.g., `DATA-303`) with an instructor until at least one student registers for it, because `StudentID` is part of the primary key and cannot be null.
- **Deletion Anomaly:** If Charlie Brown (`103`) drops out and we delete his row, we completely lose the data that `WD-202` exists and is taught by John Maxwell.

### 2. Moving to 2NF (Second Normal Form)
To reach 2NF, the table must be in 1NF and have **no partial dependencies**. This means no non-key attribute can depend on only _part_ of a composite primary key.
Our primary key is `(StudentID, CohortID)`.
- `StudentName` depends only on `StudentID` (Partial dependency).
- `Instructor` and `InstructorOffice` depend only on `CohortID` (Partial dependency).
To fix this, we split the table into two to remove partial dependencies:
#### Table A: Students

|**StudentID (PK)**|**StudentName**|
|---|---|
|101|Alice Smith|
|102|Bob Jones|
|103|Charlie Brown|
|104|Diana Prince|

#### Table B: CohortDetails

|**CohortID (PK)**|**Instructor**|**InstructorOffice**|
|---|---|---|
|COMP-101|Jane Doe|Room 302|
|WD-202|John Maxwell|Room 415|

_(Note: You would also keep a bridge table `StudentCohorts(StudentID, CohortID)` to map who is in what class, which is inherently in BCNF)._

#### Evaluating Redundancy at 2NF:
We solved the student-related anomalies. However, look closely at **Table B**. What if Jane Doe teaches multiple cohorts? Let's add a row to see the lingering redundancy:

|**CohortID (PK)**|**Instructor**|**InstructorOffice**|
|---|---|---|
|COMP-101|Jane Doe|Room 302|
|WD-202|John Maxwell|Room 415|
|**COMP-102**|**Jane Doe**|**Room 302**|

Because Jane Doe teaches two classes, the fact that she is in `Room 302` is still redundant. If she changes offices, we still have an update anomaly across multiple cohorts. This is a **transitive dependency**: `CohortID` $\rightarrow$ `Instructor` $\rightarrow$ `InstructorOffice`.

### 3. Moving to 3NF (Third Normal Form)

To reach 3NF, a table must be in 2NF and have **no transitive dependencies**. Non-key attributes must depend _only_ on the primary key, the whole key, and nothing but the key.
We eliminate the transitive dependency by splitting **Table B** into two separate tables:

#### Table B1: Cohorts

|**CohortID (PK)**|**Instructor**|
|---|---|
|COMP-101|Jane Doe|
|WD-202|John Maxwell|
|COMP-102|Jane Doe|

#### Table B2: Instructors

|**Instructor (PK)**|**InstructorOffice**|
|---|---|
|Jane Doe|Room 302|
|John Maxwell|Room 415|

#### Evaluating Redundancy at 3NF:

Every piece of data is now a single fact. If Jane Doe changes offices, we alter exactly one cell in Table B2. We can add a new instructor without assigning them a cohort yet.
For the vast majority of database designs, 3NF is completely clean. But there is a strict edge case where 3NF still permits redundancy: when a table has **overlapping candidate keys**.

### 4. Moving to BCNF (Boyce-Codd Normal Form)

BCNF is a stricter version of 3NF. The rule for BCNF is simple: **For every non-trivial functional dependency $X \rightarrow Y$, $X$ must be a superkey (a candidate key).**

Let's look at a table where 3NF fails to stop redundancy. Imagine we track a student's lab help sessions.
- Students belong to a specific major track (e.g., Full Stack, Data).
- Each lab instructor specializes in exactly _one_ track.
- A student can have multiple instructors, but only one instructor per track.
#### The 3NF Table (LabAssignments)

|**StudentID**|**Track**|**LabInstructor**|
|---|---|---|
|**101**|Full Stack|Jane Doe|
|**101**|Data|Sarah Jenkins|
|**102**|Full Stack|Jane Doe|

- **Candidate Keys / Keys:** `(StudentID, Track)` or `(StudentID, LabInstructor)`
- **Functional Dependency:** `LabInstructor` $\rightarrow$ `Track` (Since Jane Doe _only_ teaches Full Stack).
This table is technically in **3NF** because `Track` is part of a candidate key (`StudentID, Track`), meaning it is a prime attribute, and 3NF allows prime attributes to depend on non-keys.
##### The Redundancy in 3NF:
The fact that `Jane Doe` teaches `Full Stack` is repeated across rows 1 and 3.
- **Update Anomaly:** If Jane Doe switches to teaching the Data track, we have to update multiple rows.
- **Insertion Anomaly:** We cannot record that a new instructor named "Alex" teaches "DevOps" until a student explicitly books a session with them.
**Why it fails BCNF:** In the dependency `LabInstructor` $\rightarrow$ `Track`, the determinant `LabInstructor` is **not** a candidate key on its own.

#### The BCNF Solution

To fix this, we decompose the table so that every determinant is a candidate key:

### Table 1: StudentInstructors

|**StudentID**|**LabInstructor**|
|---|---|
|101|Jane Doe|
|101|Sarah Jenkins|
|102|Jane Doe|

### Table 2: InstructorSpecialties

|**LabInstructor (PK)**|**Track**|
|---|---|
|Jane Doe|Full Stack|
|Sarah Jenkins|Data|

Now, the dependency `LabInstructor` $\rightarrow$ `Track` lives in Table 2, where `LabInstructor` **is** the primary key. The redundancy is entirely eliminated, anomalies are gone, and the database is completely robust.

