 #lang eopl
;Program Mukherjee

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
    (Exp ("-" "(" Exp "," Exp ")")
         diff-Exp)
    (Exp ("*" "(" Exp "," Exp ")")
         mult-Exp)
    (Exp ("+" "(" Exp "," Exp ")")
         plus-Exp)
    (Exp ("QuoInt" "(" Exp "," Exp ")")
         div-Exp)
    (Exp ("RemInt" "(" Exp "," Exp ")")
         rem-Exp)
    (Exp ("minus" "(" Exp ")")
         minus-Exp)
    (Exp ("zero?" "(" Exp ")")
         zero-Exp?)
    (Exp ("equal?" "(" Exp "," Exp ")")
         equal-Exp?)
    (Exp ("greater?" "(" Exp "," Exp ")")
         greater-Exp?)
    (Exp ("less?" "(" Exp "," Exp ")")
         less-Exp?)
    (Exp ("if" Exp "then" Exp "else" Exp)
         if-Exp)
    (Exp (identifier)
         var-Exp)
    (Exp ("let" identifier "=" Exp "in" Exp)
         let-Exp)
    (Exp ("proc" "(" identifier ")" Exp)
         proc-Exp)
    (Exp ("(" Exp Exp ")")
         call-Exp)
    (Exp ("letrec" identifier "(" identifier ")" "=" Exp "in" Exp)
         letrec-Exp)))

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
    (Expval->Extractor (value-of-Program (scan&parse str)))))

(define Expval->Extractor
  (lambda (val)
    (cases Expval val
      (bool-val (bool)
               bool)
      (num-val (num)
               num)
      (else
       (report-Expval-extractor-error 'bool val)))))
 
;Enviroment Expression
(define-datatype environment environment?
  (empty-env)
  (extend-env (var symbol?)
              (val Expval?)
              (saved-env environment?))
  (extend-env-rec (p-name symbol?)
                  (b-var symbol?)
                  (body Exp?)
                  (env environment?)))

(define apply-env
  (lambda (env search-var)
    (cases environment env
      (empty-env () #f)
      (extend-env (var val saved-env)
                  (if (eqv? search-var var) val
                      (apply-env saved-env search-var)))
      (extend-env-rec (p-name b-var body saved-env)
                      (if (eqv? search-var p-name)
                          (proc-val (procedure b-var body env))
                          (apply-env saved-env search-var))))))                  
                  
(define init-env
  (lambda ()
    (extend-env 'i (num-val 1)
                (extend-env 'v (num-val 5)
                            (extend-env 'x (num-val 10)
                                        (empty-env))))))
;Expressed Values
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

;Continuation
(define-datatype continuation continuation?
  (end-cont)
  (zero1-cont (cont continuation?))
  (equal1-cont (exp2 Exp?)
              (env environment?)
              (cont continuation?))
  (equal2-cont (exp1 Expval?)
              (cont continuation?))
  (greater1-cont (exp2 Exp?)
              (env environment?)
              (cont continuation?))
  (greater2-cont (exp1 Expval?)
              (cont continuation?))
  (less1-cont (exp2 Exp?)
              (env environment?)
              (cont continuation?))
  (less2-cont (exp1 Expval?)
              (cont continuation?))
  (let-exp-cont (var symbol?)
                (body Exp?)
                (env environment?)
                (cont continuation?))
  (if-test-cont (exp2 Exp?)
                (exp3 Exp?)
                (env environment?)
                (cont continuation?))
  (diff1-cont (exp2 Exp?)
              (env environment?)
              (cont continuation?))
  (diff2-cont (exp1 Expval?)
              (cont continuation?))
  (plus1-cont (exp2 Exp?)
              (env environment?)
              (cont continuation?))
  (plus2-cont (exp1 Expval?)
              (cont continuation?))
  (mult1-cont (exp2 Exp?)
              (env environment?)
              (cont continuation?))
  (mult2-cont (exp1 Expval?)
              (cont continuation?))
  (div1-cont (exp2 Exp?)
              (env environment?)
              (cont continuation?))
  (div2-cont (exp1 Expval?)
              (cont continuation?))
  (rem1-cont (exp2 Exp?)
              (env environment?)
              (cont continuation?))
  (rem2-cont (exp1 Expval?)
              (cont continuation?))
  (minus1-cont (exp2 Exp?)
              (env environment?)
              (cont continuation?))
  (minus2-cont (exp1 Expval?)
              (cont continuation?)) 
  (rator-cont (rand Exp?)
              (env environment?)
              (cont continuation?))
  (rand-cont (val1 Expval?)
             (cont continuation?)))

(define apply-cont
  (lambda (cont val)
    (cases continuation cont
      (end-cont ()
                (begin
                  (eopl:printf "End of Computation.~%")
                  val))
      (zero1-cont (saved-cont)
                  (apply-cont saved-cont
                              (bool-val (zero? (Expval->num val)))))
      (equal1-cont (exp2 saved-env saved-cont)
                  (value-of/k  exp2 saved-env (equal2-cont val saved-cont)))
      (equal2-cont (val1 saved-cont)
                  (let ((num1 (Expval->num val1))
                        (num2 (Expval->num val)))
                    (apply-cont saved-cont
                                (bool-val (equal? num2 num1)))))
      (greater1-cont (exp2 saved-env saved-cont)
                  (value-of/k  exp2 saved-env (greater2-cont val saved-cont)))
      (greater2-cont (val1 saved-cont)
                  (let ((num1 (Expval->num val1))
                        (num2 (Expval->num val)))
                    (apply-cont saved-cont
                                (bool-val (> num1 num2)))))
      (less1-cont (exp2 saved-env saved-cont)
                  (value-of/k  exp2 saved-env (less2-cont val saved-cont)))
      (less2-cont (val1 saved-cont)
                  (let ((num1 (Expval->num val1))
                        (num2 (Expval->num val)))
                    (apply-cont saved-cont
                                (bool-val (< num1 num2)))))
      (let-exp-cont (var body saved-env saved-cont)
                    (value-of/k body (extend-env var val saved-env) saved-cont))
      (if-test-cont (exp2 exp3 saved-env saved-cont)
                    (if (Expval->bool val)
                        (value-of/k exp2 saved-env saved-cont)
                        (value-of/k exp3 saved-env saved-cont)))
      (minus1-cont (exp2 saved-env saved-cont)
                  (value-of/k  exp2 saved-env (minus2-cont val saved-cont)))
      (minus2-cont (val1 saved-cont)
                  (let ((num1 (Expval->num val1))
                        (num2 (Expval->num val)))
                    (apply-cont saved-cont
                                (num-val (- num2 num1)))))
      (plus1-cont (exp2 saved-env saved-cont)
                  (value-of/k  exp2 saved-env (plus2-cont val saved-cont)))
      (plus2-cont (val1 saved-cont)
                  (let ((num1 (Expval->num val1))
                        (num2 (Expval->num val)))
                    (apply-cont saved-cont
                                (num-val (+ num1 num2)))))
      (diff1-cont (exp2 saved-env saved-cont)
                  (value-of/k  exp2 saved-env (diff2-cont val saved-cont)))
      (diff2-cont (val1 saved-cont)
                  (let ((num1 (Expval->num val1))
                        (num2 (Expval->num val)))
                    (apply-cont saved-cont
                                (num-val (- num1 num2)))))
      (mult1-cont (exp2 saved-env saved-cont)
                  (value-of/k  exp2 saved-env (mult2-cont val saved-cont)))
      (mult2-cont (val1 saved-cont)
                  (let ((num1 (Expval->num val1))
                        (num2 (Expval->num val)))
                    (apply-cont saved-cont
                                (num-val (* num2 num1)))))
      (div1-cont (exp2 saved-env saved-cont)
                  (value-of/k  exp2 saved-env (div2-cont val saved-cont)))
      (div2-cont (val1 saved-cont)
                  (let ((num1 (Expval->num val1))
                        (num2 (Expval->num val)))
                    (apply-cont saved-cont
                                (num-val (quotient num1 num2)))))
      (rem1-cont (exp2 saved-env saved-cont)
                  (value-of/k  exp2 saved-env (rem2-cont val saved-cont)))
      (rem2-cont (val1 saved-cont)
                  (let ((num1 (Expval->num val1))
                        (num2 (Expval->num val)))
                    (apply-cont saved-cont
                                (num-val (remainder num1 num2)))))
      (rator-cont (rand env cont)
                  (value-of/k rand env
                              (rand-cont val cont)))
      (rand-cont (val1 cont)
                 (let ((proc1 (Expval->proc val1)))
                   (apply-procedure/k proc1 val cont))))))

;Value-of Continuation
(define value-of-Program
  (lambda (pgm)
    (cases Program pgm
      (a-Program (Exp)
                 (value-of/k Exp (init-env) (end-cont))))))

(define value-of/k
  (lambda (exp env cont)
    (cases Exp exp
      (const-Exp (num)
                 (apply-cont cont (num-val num)))
      (var-Exp (var)
               (apply-cont cont (apply-env env var)))
      (proc-Exp (var body)
                (apply-cont cont
                            (proc-val (procedure var body env))))
      (letrec-Exp (p-name b-var p-body letrec-body)
                  (value-of/k letrec-body
                              (extend-env-rec p-name b-var p-body env) cont))
      (zero-Exp? (exp1)
                 (value-of/k exp1 env (zero1-cont cont)))
      (equal-Exp? (exp1 exp2)
                (value-of/k exp1 env
                            (equal1-cont exp2 env cont)))
      (greater-Exp? (exp1 exp2)
                (value-of/k exp1 env
                            (greater1-cont exp2 env cont)))
      (less-Exp? (exp1 exp2)
                (value-of/k exp1 env
                            (less1-cont exp2 env cont)))
      (if-Exp (exp1 exp2 exp3)
              (value-of/k exp1 env
                          (if-test-cont exp2 exp3 env cont)))
      (let-Exp (var exp1 body)
               (value-of/k  exp1 env
                            (let-exp-cont var body env cont)))
      (diff-Exp (exp1 exp2)
                (value-of/k exp1 env
                            (diff1-cont exp2 env cont)))
      (plus-Exp (exp1 exp2)
                (value-of/k exp1 env
                            (plus1-cont exp2 env cont)))
      (mult-Exp (exp1 exp2)
                (value-of/k exp1 env
                            (mult1-cont exp2 env cont)))
      (div-Exp (exp1 exp2)
                (value-of/k exp1 env
                            (div1-cont exp2 env cont)))
      (rem-Exp (exp1 exp2)
                (value-of/k exp1 env
                            (rem1-cont exp2 env cont)))
      (minus-Exp (exp1)
                (value-of/k exp1 env
                            (minus1-cont (const-Exp 0) env cont)))                            
      (call-Exp (rator rand)
                (value-of/k rator env
                            (rator-cont rand env cont))))))

;Procedure Datatype
(define-datatype proc proc?  
  (procedure (var symbol?)
             (body Exp?)
             (saved-env environment?)))

(define apply-procedure/k
  (lambda (proc1 val cont)
    (cases proc proc1                     
      (procedure (var body saved-env)
                 (value-of/k body (extend-env var val saved-env) cont )))))      


(define report-error
  (lambda ()
    (eopl:error 'Exp "Error")))


;SUDO TEST CASES
(define test
  "let x = 5 in x")

(define test_1
  "letrec double (x)
                  = if zero?(x)
                    then 0
                    else -(( double - (x,1)), -2)
                    in (double 6)")

(define test_2_
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

(define exp
  "let x = 3
      in let y = -(x,3)
           in if zero?(y) then -1 else y")
rr
(define f1
  "letrec f(x) = proc (y) proc (z) -(-(x,y),-(0,z)) in (((f 3)2)4)")

(define f3
  "letrec f(x) = let y = -(x,5) in proc (z) - (y,z) in let y = 8 in ((f 7)y)")



                
           
             
    
    

