#lang eopl

;> (define var (var-exp 'x))
;> (define body (app-exp
;                 (var-exp 'x) (app-exp
;                                (var-exp 'y)(app-exp
;                                             (var-exp 'z)(var-exp 'a)))))
;> (define lc-exp (lambda-exp 'x body))
;> (occurs-free? 'x lc-exp) ;#f
;> (occurs-free? 'y lc-exp) ;#t

;Lc-Exp: Identifier
;      : (lambda (identifier) Lc-Exp
;      : (Lc-Exp Lc-Exp)

(define-datatype lc-exp lc-exp?
  (var-exp
   (var symbol?))
  (lambda-exp
   (bound-var symbol?)
   (body lc-exp?))
  (app-exp
   (rator lc-exp?)
   (rand lc-exp?)))

;occurs-free? : Sym x LcExp --> bool
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

;parse-expression: SchemeVal --> LcExp
(define parse-expression
  (lambda (datum)
    (cond
      ((symbol? datum) (var-exp
                        datum))
      ((pair? datum) (if (eqv? (car datum) 'lambda)
                         (lambda-exp
                          (car (cadr datum))
                          (parse-expression (caddr datum)))
                         (app-exp
                          (parse-expression (car datum))
                          (parse-expression (cadr datum)))))
      (else (report-invalid-concrete-syntax datum)))))

(define report-invalid-concrete-syntax
  (lambda (datum)
    (eopl:error 'parse-expression "Invalid Lc-Exp Syntax: ~s ~%" datum)))

;unparse-lc-exp: LcExp --> SchemeVal
(define unparse-lc-exp
  (lambda (exp)
    (cases lc-exp exp
      (var-exp (var)
               var)
      (lambda-exp (bound-var body)
                  (list 'lambda (list bound-var)
                        (unparse-lc-exp body)))
      (app-exp (rator rand)
               (list
                (unparse-lc-exp rator)
                (unparse-lc-exp rand))))))
      


            
                       



        







  
