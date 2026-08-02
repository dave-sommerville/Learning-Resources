## Toolbox
### String Tools
Strings are essentially readonly char arrays. This means that you can iterate through the individual characters using a for loop or a foreach loop. Strings cannot be modified this way, however. 
In other words, you can read `str[i]`, but you can't assign it a value like  `str[i] = 'a'` 
#### Char Arrays
To modify a string in place you must convert it to a true array using `char[] charArr =  str.ToCharArray();` To get a string back from that char array, you can simply initialize a new string like `string str = new string(charArr);` 
#### Char Methods
Just like strings, chars have their own instance methods. There are two types to be primarily concerned with 

**Case Management**
`char.ToLowerInvariant('c');` to change the character to lowercase or `char.ToUpperInvariant('c');` to change the character to uppercase
To simply check to see the case of the char, you can use `char.IsUpper('c')` or `char.IsLower('c')`
**Character interpretation**
Instead of using a complex regex, you can use char methods to see certain values such as 
`char.IsLetterOrDigit('c')` or more specific variations such as `char.IsLetter('c')` and `char.IsDigit('c')`
#### StringBuilder
Because strings are immutable, modifying a string inside a loop (like `str += "a"`) creates a brand-new string in memory on every iteration. This quickly leads to a "Time Limit Exceeded" (TLE) error on LeetCode. StringBuilder acts as a mutable string buffer.
It appends or modifies characters in $O(1)$ amortized time. Always use StringBuilder if you are constructing a string dynamically inside a loop, then call `.ToString()` at the very end.
```
StringBuilder sb = new StringBuilder();
for(int i = 0; i < 10; i++) {
	sb.Append($"Stage: {i}");
}
string result = sb.ToString();
```

### Math Tools
**`Math.Max()` / `Math.Min()`**
- Instead of writing an `if-else` block to find the larger or smaller of two numbers, these methods do it in a single line.
**`Math.Abs()`**
- Returns the absolute (positive) value of a number.
**`Math.Pow(base, exponent)`**
- Computes a number raised to a power. Note: For squaring, `x * x` is faster and avoids casting to a double
**`Math.Sqrt(x)`**
- Calculates the square root of a number.
**`Math.Ceiling()` / `Math.Floor()`**
- `Math.Floor()` rounds down toward negative infinity; `Math.Ceiling()` rounds up toward positive infinity.

## Common Patterns

### Two pointers
The **Two Pointers** pattern uses two integer variables (acting as array indices) to iterate through a data structure at the same time, often moving toward each other or at different speeds.
#### Loops

**for**

```
int j = 0;
				// i moves right
for(int i = 0; i < n; i++) {
	SomeLogic();
	j--; // j moves left
};
```

**while**
```
int left = 0;
int right = n;
while (left < right) {
	SomeLogic();
	left++;  // left moves right
	right--; // right moves left
}
```

#### Recursion
The Two Pointer pattern can also be followed using recursion, however the pointer values must be passed as parameters into the recursive methods signature and progressed in the recursive call
```
public bool RecursiveAlwaysTrue(int left = 0, int right = n) {
	if (left > right) return true;
	SomeLogic(); // With a false condition for example
	return RecursiveAlwaysTrue(left + 1, right - 1);
}
```

### Temp Variable
When there is some type of changing of data, you will often rely on the temp variable pattern, especially in the case of string or array reversals. Inside a loop, a temp variable is created much like how you would use a placeholder in real life. 

Values are held in two variables, with a declared temp variable 

`int a = 1;` 
`int b = 2;`
`int temp;`
![](LeetCode%20Media/temp-variable-01.jpg | 300)

One value is moved into the placeholder position 
`temp = a;`
![](LeetCode%20Media/temp-variable-02.jpg | 300)

The other value gets moved to the freely available spot 
`a = b;`
![](LeetCode%20Media/temp-variable-03.jpg | 300)

Finally the original value is moved from the placeholder back to the switched position
`b = temp;`
![](LeetCode%20Media/LeetCode%20Media/temp-variable-04.jpg |  300)


#### Binary Tree Diagram
See the below example of a binary tree for:
root = \[2, 7, 5, 2, 6, null, 9,null, null, 5, 11, 4, null\]
Note the class structure of TreeNode
![](LeetCode%20Media/binary-tree.png)

## Examples
### Valid Palindrome 
LeetCode Problem: [125. Valid Palindrome](https://leetcode.com/problems/valid-palindrome/)
#### Iterative Approaches
**for loop**
![](LeetCode%20Media/palindrome-for.png)
**while loop**
![](LeetCode%20Media/palindrome-while.png)
#### Recursive Approach
![](LeetCode%20Media/palindrome-recursive.png)


### Reverse String 
LeetCode Problem: [344. Reverse String](https://leetcode.com/problems/reverse-string/)
#### Iterative
![](LeetCode%20Media/reverse-str-while.png)
#### Recursive
![](LeetCode%20Media/reverse-str-recursive.png)

### Maximum Depth
LeetCode Problem: [104. Maximum Depth of Binary Tree](https://leetcode.com/problems/maximum-depth-of-binary-tree/)
#### Iterative Approach (brother, ew)
![](LeetCode%20Media/max-depth-iterative.png)
#### Recursive Approach (elegant, beautiful)
![](LeetCode%20Media/max-depth-recursive.png)















