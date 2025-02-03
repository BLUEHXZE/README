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
        list => acc => {
            if (list.kind === "nil") {
                return acc; // Return the accumulated result if the list is empty
            }
            return reduceList(reducer)(list.next)(reducer(list.value)(acc)); // Apply the reducer and continue
        }

//Q5 B (do not modify the signature of function removeRepetitions)
export const removeRepetitions = <T> (list : List<T>) : List<T> => {
    const seen = new Set<T>(); // Set to track seen values
    const result: List<T> = empty(); // Start with an empty list

    const addUnique = (current: List<T>): List<T> => {
        if (current.kind === "nil") {
            return result; // Return the result if the current list is empty
        }
        if (!seen.has(current.value)) {
            seen.add(current.value); // Add value to the set if not seen
            return full(current.value)(addUnique(current.next)); // Add to result and continue
        }
        return addUnique(current.next); // Skip duplicates
    };

    return addUnique(list); // Start the process
}
