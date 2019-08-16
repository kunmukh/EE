#lang eopl
;Program
;PROC LANG Mukherjee

;scanner
(define scanner-spec-let
  '((white-sp (whitespace) skip)
    (comment ("%" (arbno (not #\newline))) skip)
    (identifier (letter (arbno (or letter digit))) symbol)
    (number ((or "-" "+" digit)(arbno digit)) number)))

(define grammar-let
  '((Program (Exp)
             a-Program)
    (Exp (number)
         const-Exp)
    (Exp (identifier)
         var-Exp)
    (Exp ("-" "(" Exp "," Exp ")")
         diff-Exp)
    (Exp ("zero?" "(" Exp ")")
         zero-Exp?)
    (Exp ("if" Exp "then" Exp "else" Exp)
         if-Exp)    
    (Exp ("let" identifier "=" Exp "in" Exp)
         let-Exp)
    (Exp ("proc" "(" identifier ")" Exp)
         proc-Exp)
    (Exp ("(" Exp Exp ")")
         call-Exp)))

(sllgen:make-define-datatypes scanner-spec-let grammar-let)

(define list-the-datatypes
  (lambda ()
    (sllgen:list-define-datatypes scanner-spec-let grammar-let)))

(define just-scan
  (sllgen:make-string-scanner scanner-spec-let grammar-let))

(define scan&parse
  (sllgen:make-string-parser scanner-spec-let grammar-let))

(define run
  (lambda (str)
    (value-of-Program (scan&parse str))))

 
;Enviroment Expression
(define-datatype environment environment?
  (empty-env)
  (extend-env (var symbol?)
              (val Expval?)
              (saved-env environment?)))

(define apply-env
  (lambda (search-var env)
    (cases environment env
      (empty-env ()
                 #f)
      (extend-env (var val saved-env)
                  (if (eqv? search-var var)
                      val
                      (apply-env search-var saved-env))))))

(define init-env
  (lambda ()
    (extend-env 'i (num-val 1)
                (extend-env 'v (num-val 5)
                            (extend-env 'x (num-val 10)
                                        (empty-env))))))
(define test-env
  (lambda ()
    (empty-env)))

(define test-3
  (lambda ()
    (extend-env 'x (num-val 10) (empty-env))))
        

;Expressed and Denoted Value
(define-datatype Expval Expval?
  (num-val (num number?))
  (bool-val (bool boolean?))
  (proc-val (proc proc?)))

(define Expval->num
  (lambda (val)
    (cases Expval val
      (num-val (num)
               num)
      (else
       (report-Expval-extractor-error 'num val)))))

(define Expval->bool
  (lambda (val)
    (cases Expval val
      (bool-val (bool)
               bool)
      (else
       (report-Expval-extractor-error 'bool val)))))

(define Expval->proc
  (lambda (val)
    (cases Expval val
      (proc-val (proc)
               proc)
      (else
       (report-Expval-extractor-error 'proc val)))))

(define report-Expval-extractor-error
  (lambda (type val)
    (eopl:error 'Exp-val "Error extracting ~s from ~s" type val)))

;PROC Langauge

#|(define-datatype Program Program?
  (a-Program (Exp1 Exp?)))

(define-datatype Exp Exp?
  (const-Exp (num number?))
  (diff-Exp (Exp1 Exp?)
            (Exp2 Exp?))
  (zero-Exp? (Exp1 Exp?))
  (if-Exp (Exp1 Exp?)
          (Exp2 Exp?)
          (Exp3 Exp?))
  (var-Exp (var symbol?))
  (let-Exp (var symbol?)
           (Exp1 Exp?)
           (body Exp?))
  (proc-exp (var symbol?)
            (body Exp?))
  (call-exp (rator Exp?)
            (rand Exp?)))|#

(define-datatype proc proc?
  (procedure (bound-var symbol?)
             (body Exp?)
             (saved-env environment?)))

(define apply-procedure
  (lambda (proc1 val)
    (cases proc proc1
      (procedure (bound-var body saved-env)
                 (value-of body (extend-env bound-var val saved-env))))))


;Interpreter
(define value-of-Program
  (lambda (pgm)
    (cases Program pgm
      (a-Program (Exp)
                 (value-of Exp (init-env))))))

(define value-of
  (lambda (Exp1 env)
    (cases Exp Exp1
      (const-Exp (num)
                 (num-val num))
      (var-Exp (var)
               (apply-env var env))
      (diff-Exp (Exp1 Exp2)
                (let ((val1 (value-of Exp1 env))
                      (val2 (value-of Exp2 env)))
                  (let ((num1 (Expval->num val1))
                        (num2 (Expval->num val2)))
                    (num-val
                     (- num1 num2)))))
      (zero-Exp? (Exp1)
                 (let ((val1 (value-of Exp1 env)))
                   (let ((num1 (Expval->num val1)))
                     (if (zero? num1)
                         (bool-val #t)
                         (bool-val #f)))))
      (if-Exp (Exp1 Exp2 Exp3)
              (let ((val1 (value-of Exp1 env)))
                (if (Expval->bool val1)
                    (value-of Exp2 env)
                    (value-of Exp3 env))))
      (let-Exp (var Exp1 body)
               var)
      (proc-Exp (bound-var body)
                (proc-val (procedure bound-var body env)))
      (call-Exp (exp1 exp2)
                (let ((rator (Expval->proc (value-of exp1 env)))
                      (rand (value-of exp2 env)))
                  (apply-procedure rator rand))))))


(define report-error
  (lambda ()
    (eopl:error 'Exp "Error")))


;TEST CASES
(define test
  "let x = 5 in x")

(define test_2
  "let f = proc (y) -(y, 3) in (f 10)")

(define test_3
  "let x = 10 in - (x, 3)")

(define test_4
  "let x = 200
   in let f = proc (z) -(z,x)
      in let x = 100
         in let g = proc (z) -(z,x)
            in -((f 1) , (g 1))")

(define multiplier
  "let makemult = proc (maker)
                  proc (x)
                  if zero? (x)
                  then 0
                  else - ( ( (maker maker) -(x,1) ), -4 )
                  in let times4 = proc (x) ((makemult makemult) x)
                     in (times4 3)")

(define times
  "let makemult = proc (y) proc (maker)
                  proc (x)
                  if zero? (x)
                  then 0
                  else - (((maker maker) -(x,1)), -(0,y))
                  in let times = proc (y)
                                 proc (x)
                                 (((makemult y) (makemult y)) x)
                                 in ((times 10) 20)")

(define factorial
  "let makemult = proc (y) proc (maker)
                  proc (x)
                  if zero? (x)
                  then 0
                  else - (((maker maker) -(x,1)), -(0,y))
                  in let times = proc (y)
                                 proc (x)
                                 (((makemult y) (makemult y)) x)
                                 in let prodmake = proc (maker)
                                                   proc (x)
                                                   if zero? (x)
                                                   then 1
                                                   else (( times (( maker maker) -(x,1))) x)
                                                   in let fact = proc (x) ((prodmake prodmake) x)
                                                       in (fact 5)")

(define sum
  "let maker = proc (maker)
               proc (x)
               if zero? (x)
               then 0
               else - (((maker maker) -(x,1)), -(0,x))
               in let sum = proc (x) ((maker maker) x)
                  in (sum 10)")

           
             
    
    

