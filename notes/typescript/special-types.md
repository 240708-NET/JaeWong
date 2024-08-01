# Special Types and Objects Types


### Special Types

* `unknown`: Represents any value but is safer because it’s not legal to do anything with the value.
    ```
    function f2(a: unknown) {
        a.b();    // Error: 'a' is of type 'unknown'.
    }
    ```

* `never`: Use for functions that never return a value
    ```
    function fail(msg: string): never {
        throw new Error(msg);
    }
    ```

* `any`: Use to bypass typechecking errors.
    ```
    let obj: any = { x: 0 };
    // None of the following lines of code will throw compiler errors.
    // Using `any` disables all further type checking, and it is assumed
    // you know the environment better than TypeScript.
    obj.foo();
    obj();
    obj.bar = 100;
    obj = "hello";
    ```

* `undefined` and `null`: primitive values used to signal absent or uninitialized value.


### Object Types
The fundamental way to group and pass around data.

1. Interface
    ```
    interface Person {
        name: string;
        age: number;
    }
    ```

2. Type Alias
    ```
    type Person = {
        name: string;
        age: number;
    };
    ```

3. Anonymous Type
    ```
    function greet(person: { name: string; age: number }) {
        return "Hello " + person.name;
    }
    ```
