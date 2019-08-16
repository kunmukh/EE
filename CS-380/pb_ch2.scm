#lang eopl

;2.9 2.21 2.22 2.23 2.29 2.31

#|(define empty-env
  (lambda ()
    (list 'empty-env)))

(define extend-env
  (lambda (var val env)
    (list 'extend-env var val env)))

(define apply-env
  (lambda (env search-var)
    (cond
      ((eqv? (car env) 'empty-env)
       (report-no-binding-found search-var))
      ((eqv? (car env) 'extend-env)
       (let ((saved-var (cadr env))
             (saved-val (caddr env))
             (saved-env (cadddr env)))
         (if (eqv? search-var saved-var)
             saved-val
             (apply-env saved-env search-var))))
       (else
        (report-invalid-env env)))))

(define report-no-binding-found
  (lambda (search-var)
    (eopl:error 'apple-env "No binding for ~s" search-var)))

(define report-invalid-env
  (lambda (env)
    (eopl:error 'apply-env "Bad Environment: ~s" env)))

;9
(define has-binding?
  (lambda (env s)
    (if (eqv? (car env) 'empty-env)
        #f
        (if (eqv? (cadr env) s)
            (caddr env)
        (has-binding? (caddr env) s)))))|#

;2.21
;env ::= empty-env //empty-env
;    ::= extend-env (var val env) //extend-env
(define-datatype env env?
  (empty-env)
  (extend-env
   (var symbol?)
   (val number?)
   (env env?)))

(define has-binding?
  (lambda (s-env sym)
    (cases env s-env
      (empty-env ()
                 #f)
      (extend-env (var val c-env)
                  (if (eqv? var sym)
                      #t
                      (has-binding? c-env sym))))))
;(define t-env (extend-env 'x 1 (extend-env 'y 2 (extend-env 'z 3.2 (empty-env)))))
;(has-binding? t-env 'x)

;2.22
;stack ::= empty-stack
;      ::= push (val)
;      ::= pop (val)

(define-datatype stack stack?
  (empty-stack)
  (push-stack
   (saved-var number?)
   (saved-stack stack?)))

(define pop
  (lambda (stk)
    (cases stack stk
      (empty-stack ()
                   (report-empty-stack))
      (push-stack (saved-val saved-stack)
                  saved-stack))))  

(define top
  (lambda (stk)
    (cases stack stk
      (empty-stack ()
                   report-empty-stack)
      (push-stack (saved-val saved-stack)
                  saved-val))))

(define empty-stack?
  (lambda (stk)
    (cases stack stk
      (empty-stack ()
                   #t)
      (push-stack (saved-val saved-stack)
                  #f))))
    

(define report-empty-stack
  (lambda (stk)
    (eopl:error 'top "Stack is Empty ~%")))
;>(define stk (push-stack 9 (push-stack 8 (empty-stack))))
;>(top stk)
;>define kun (pop stk))
;:>(top kun)

;2.23
;Lc-Exp: Identifier
;      : (lambda (identifier) Lc-Exp
;      : (Lc-Exp Lc-Exp)

(define-datatype lc-exp lc-exp?
  (var-exp
   (var identifier?))
  (lambda-exp
   (bound-var identifier?)
   (body lc-exp?))
  (app-exp
   (rator lc-exp?)
   (rand lc-exp?)))

(define identifier?
  (lambda (exp)
   (and
    (symbol? (exp)) (not (eqv? exp 'lambda)))))

(define occurs-free?
  (lambda (search-var exp)
    (cases lc-exp exp
      (var-exp (var)
               (eqv? var search-var))
      (lambda-exp (bound-var body)
                  (and
                   (not (eqv? search-var bound-var))
                   (occurs-free? search-var body)))
      (app-exp (rator rand)
               (or
                (occurs-free? search-var rator)
                (occurs-free? search-var rand))))))

;2.29
;Lc-exp::= Identifier
;          |var-exp (var)|
;        = (lambda ({Identifier}*) Lc-exp)
;          |lambda-exp (bound-vars body)|
;        = (Lc-exp {Lc-exp}*)
;          |app-exp (rator rands)|

(define-datatype lc-exp_new lc-exp_new?
  (var-exp_new
   (var identifier?))
  (lambda-exp_new 
   (bound-vars (list-of identifier?))
   (body lc-exp_new?))
  (app-exp_new 
   (rator lc-exp_new?)
   (rands (list-of lc-exp_new?))))

(define parse-expression_new
  (lambda (datum)
    (cond
      ((identifier? datum) (var-exp_new
                            datum))
      ((pair? datum) (if (eqv? (car datum) 'lambda)
                         (lambda-exp_new
                          (car (cadr datum))
                          (parse-expression_new (caddr datum)))
                     (app-exp
                      (parse-expression_new (car datum))
                      (parse-expression_new (cadr datum)))))
      (else (report-invalid-concrete-syntax datum)))))

(define report-invalid-concrete-syntax
  (lambda (datum)
    (eopl:error 'parse-expression "Invalid Lc-Exp Syntax: ~s ~%" datum)))
      
    
       

           
                 
           
























