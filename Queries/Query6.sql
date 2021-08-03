SELECT ISBN, Count(AcquireList.Resolved) as UnresolvedStock from AcquireList
    where Resolved=0
    group by ISBN
    order by UnresolvedStock desc