# LIRA
The world's most lightweight Issue-Tracking System

## Assumtions



## Data structures

Both the Users and Issues will be stored as Dictionaries since they allow for O(1) lookup
which is most of what the application is required to do.
For the getIssues method one might consider using a k-d-tree to store the issues to be able to index all attributes
and not only the id as in the Dictionary, I decided against this though as it seemed to be a bit out of scope for this case.

