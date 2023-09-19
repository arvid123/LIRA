# LIRA
The world's most lightweight Issue-Tracking System

## Assumptions

- Children with Done parents can be undone.

- No positive tests of parent relationship was done since there is currently no effect of these relationships on the outgoing operations.

- If further information about users should be available that would be gotten through more operations that do not currently exist in the program, i.e. getting assigned issues for a user is part of further development.

- Exceptions are thrown as soon as anything unintended is done, if a user of the board wants to ignore the exceptions they can do that in their own code.

## Data structures

Both the Users and Issues will be stored as Dictionaries since they allow for O(1) lookup
which is most of what the application is required to do.
For the getIssues method one might consider using a k-d-tree to store the issues to be able to index all attributes
and not only the id as in the Dictionary, I decided against this though as it seemed to be a bit out of scope for this case.

