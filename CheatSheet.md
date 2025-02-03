# Web Development Final Exam

# Cheat Sheet / Hints / Examples 

## An example of a React function component:

```ts
export const AFunctionComponent = (props: {}) : JSX.Element  => 
{
  const [something, setSomething] = React.useState<TypeOfSomething>(initialValueOfSomething)
  return (<div> Hello world!</div>)
}
```

## An example of a React class component:

```ts
export class AClassComponent extends React.Component <PropsType,StateType>{
  constructor(props:PropsType){
    super(props)
    this.state = //initial state
  }
  render():JSX.Element{
   return (<div> Hello world!</div>)
  }
}
```

## An example of radio button group

```ts
<div>Please select your preferred contact method:</div>

<div>
  <input type="radio" name="contact" value="email" checked = {/*If this boolean value is true then the radio button is checked*/} onChange={/*This is the onchange event of the group*/}/> Email
  <input type="radio" name="contact" value="phone" checked = {} onChange={}/> Phone
  <input type="radio" name="contact" value="mail" checked = {} onChange={}/> Mail
</div>
```
for the rendering of the multiple choices make use of `map((_, i) =>…)` where the key _i_ can be used to know which choice is rendered, for instance the parameter checked (boolean) to mark the chosen radio button will be 
true when the key _i_ will be equal to the relevant field (chosen) in the state. 

# An Example button

```ts
<button onClick={e=> /*what will happen after the click */}>Text of the button</button>
```

# An example of input element

```ts
//Input a date: 
<input type="date" value={/*...*/}
   onChange={/*event.currentTarget.value has the user input and you can create a new Date() using this value and save it accordingly* /}
/>

//Input a number: 
<input type="number" value={/*...*/}
   onChange={/* event.currentTarget.valueAsNumber has the user input/}
/>
```

# Confirmation Dialog:
```ts
if(window.confirm("Is the answer correct")) {
    alert("Correct!")}
else
    alert("Incorrect!")} 
```

## fetch function:

fetch() is an **asynchronous** function returning a `Promise` of a `Response` type

```ts
function fetch(
  input: string | URL | globalThis.Request,
  init?: RequestInit,
): Promise<Response>
```

The returned  `Response` object  will have a `json` field with the expected data.

## An example of an Option type

```ts
type Option<T> = { kind : "None"} | {kind: "Some", value: T}
```


