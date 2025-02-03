import { useState, useEffect } from "react";
import React from "react";
import * as App from "./App";
import * as LinkedList from "./List";

export type QuestionBase = {
  id: number,
  statement: string,
} 
type Open = QuestionBase & { kind: "open", answer?: string }
type Mcq = QuestionBase & { kind: "mcq", choices: string[], correctAnswer: number, chosen: number }

export type Question = Open | Mcq

type List<T> = LinkedList.List<T>
const empty = LinkedList.empty;  //constructor empty node
const full = LinkedList.full;    //constructor list node
const list2String = LinkedList.list2String;

const reduceList = LinkedList.reduceList;
const removeRepetitions = LinkedList.removeRepetitions;

// Implementing QuizCard Component
type QuizCardProps = Question;

export class QuizCard extends React.Component<QuizCardProps, { answer: string, feedback: string }> {
  constructor(props: QuizCardProps) {
    super(props);
    this.state = { answer: "", feedback: "" };
  }

  handleAnswerSubmission = (answer: string) => {
    this.setState({ answer });
    const isCorrect = this.props.kind === "open" ? this.props.answer === answer : false; // Adjust logic for open questions
    this.setState({ feedback: isCorrect ? "Correct!" : "Incorrect, try again." });
    console.log("Answer submitted:", answer);
  };

  render(): JSX.Element {
    return (
      <div>
        <h3>{this.props.statement}</h3>
        <input type="text" onBlur={(e) => this.handleAnswerSubmission(e.target.value)} />
        <p>{this.state.feedback}</p> {/* Display feedback */}
      </div>
    );
  }
}

// Implementing Navigator Component
type NavigatorProps = { Questions: Question[] }
type NavigatorState = { Questions: Question[], idx: number }

export const Navigator = (props: NavigatorProps): JSX.Element => {
  const [currentIdx, setCurrentIdx] = useState(0);

  const handleNext = () => {
    if (currentIdx < props.Questions.length - 1) {
      setCurrentIdx(currentIdx + 1);
    }
  };

  const handlePrevious = () => {
    if (currentIdx > 0) {
      setCurrentIdx(currentIdx - 1);
    }
  };

  const validateAnswer = (chosenAnswer: string) => {
    const currentQuestion = props.Questions[currentIdx];
    if (currentQuestion.kind === "mcq") {
      return currentQuestion.choices[currentQuestion.correctAnswer] === chosenAnswer;
    }
    return currentQuestion.answer === chosenAnswer; // Validate for open questions
  };

  const resetQuiz = () => {
    setCurrentIdx(0);
  };

  return (
    <div>
      <QuizCard {...props.Questions[currentIdx]} />
      <button onClick={handlePrevious} disabled={currentIdx === 0}>Previous</button>
      <button onClick={handleNext} disabled={currentIdx === props.Questions.length - 1}>Next</button>
      <button onClick={resetQuiz}>Reset Quiz</button>
    </div>
  );
};

// Fetching Questions
type Option<T> = { kind: 'some', value: T } | { kind: 'none' }
export const fetchQuestions = (): Promise<Option<Question[]>> => 
  new Promise((resolve) => {
    try {
      resolve({ kind: 'some', value: App.DummyQuestions });
    } catch (error) {
      resolve({ kind: 'none' }); // Handle error
    }
  });

// Testing Typescript
export const TestTypescript = (): JSX.Element => {
  const numberList: List<number> = full(256)(full(42)(full(300)(empty())));
  const reducerFunctionString: (_: number) => (__: string) => string = n => s => s === "" ? `${n}` : `${s} -> ${n}`;
  const resultA1: string = reduceList(reducerFunctionString)(numberList)("");
  
  return (
    <div>
      {`Input (List<number>): ${list2String(numberList)("")}`}
      <div>Results:</div>
      <div>{`Result Q5A (string): ${resultA1}`}</div>
    </div>
  );
}
