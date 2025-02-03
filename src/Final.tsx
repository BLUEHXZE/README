import { useState } from "react";
import React = require("react");
import * as App from "./App"
import { validate } from "webpack";
import * as LinkedList from "./List"

export type QuestionBase = {
  id: number,
  statement: string,
} 
type Open = QuestionBase & { kind: "open", answer?: string }
type Mcq = QuestionBase & { kind: "mcq", choices: string[], correctAnswer: number, chosen: number }

export type Question = Open | Mcq

type List<T> = LinkedList.List<T>
const empty = LinkedList.empty  //constructor empty node
const full = LinkedList.full    //constructor list node
const list2String = LinkedList.list2String

const reduceList = LinkedList.reduceList
const removeRepetitions = LinkedList.removeRepetitions


//ToDo 2: QuizCard 

export class QuizCard extends React.Component <Question, Question>{
	constructor(props:Question){
			super(props)
			this.state = {...props}
	}

	render():JSX.Element{ return <></>}
}


//ToDo 3: Navigator

type NavigatorProps = {Questions: Question[]}
type NavigatorState = {Questions: Question[], idx: number}

export const Navigator = (props: NavigatorProps) : JSX.Element  => 
	{
			return <></>
	}

//ToDo 4: FetchQuestions 
//Change the signature and the implementation of the function fetchQuestions:
type Option<T> = { kind: 'some', value: T } | { kind: 'none' }
export const fetchQuestions = () : Promise<Option<Question[]>> => 
   new Promise((resolve, reject) => resolve({kind: 'some', value: App.DummyQuestions}))


//ToDo 5: Typescript (implement functions reduceList and removeRepetitions in List.ts)

//Testing Typescript

export const TestTypescript = () : JSX.Element  => {

    const numberList:List<number> = full(256)(
                                     full(42)(
                                      full(42)(
                                       full(300)(
																																								full(300)(
																																									full(300)(
																																										full(300)(
																																											full(140)(
																																												full(154)(
																																													full(154)(
																																														full(154)(
																																															full(154)(
																																																full(2)(
																																																	empty()
																																																)
																																															)
																																														)
																																													)
																																												)
																																											)
																																										)
																																									)
																																								)
																																							)
																																						)
																																					)
																																				)
																																			
    

    const reducerFunctionString : (_: number) => (__: string) => string = n => s => s == "" ? `${n}` : `${s} -> ${n}` 
				const reducerFunctionNumber: (_: number) => (__: number) => number = a => b => a + b

    const resultA1: string = reduceList(reducerFunctionString)(numberList)("") //test1 of Q5A
				const resultA2: number = reduceList(reducerFunctionNumber)(numberList)(0) //test2 of Q5A

				const resultB: List<number> = removeRepetitions(numberList) //test of Q5B

    return (
      <div> 
								{`         Input (List<number>): ${list2String(numberList)("")}` }
								<div>Results:</div>
        <div> 
								  {`   => Result Q5A (string) : ${resultA1}` } 
        </div>
								<div> 
								  {`   => Result Q5A (number) : ${resultA2}` } 
        </div>
        <div> 
										{`   => Result Q5B (List<number>) : ${list2String(resultB)("")}` }
        </div>

      </div>

  );

}


