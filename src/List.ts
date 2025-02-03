
export type Empty = { kind: "nil" }
export type Full<T> = { kind: "cons", readonly value: T, readonly next: List<T> }
export type List<T> = Full<T> | Empty

//constructor empty node:
export const empty = () : Empty => ({kind: "nil"}) 

//constructor list node:
export const full = <T>(val: T) : (tail: List<T>) => List<T> => 
    tail => ({ kind: "cons", value: val, next: tail })

//from list to string:
export const list2String = <T>(list : List<T>) : (acc: string) => string => acc => 
    acc == "" ? 
    list.kind == "nil" ? "[" + acc + "]":
    list2String(list.next)(`[${list.value}`) :
        list.kind == "nil" ? acc + "]":
        list2String(list.next)(`${acc}, ${list.value}`)


//Q5 A (do not modify the signature of function reduceList)
export const reduceList = <T,R> (reducer: (el: T) => (res: R) => R) : (list: List<T>) => (acc: R) => R => 
        list => acc => 
            //ToDo...
            acc //remove this line, then implement the function from here
           


//Q5 B (do not modify the signature of function removeRepetitions)
export const removeRepetitions = <T> (list : List<T>) : List<T> => 
            //ToDo...
            empty() //remove this line, then implement the function from here
  