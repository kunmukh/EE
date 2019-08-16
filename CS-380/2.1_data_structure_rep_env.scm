;Kunal Mukherjee

#lang eopl

(define empty-env
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
    (if (eqv? (car env) 'empty-env) #f
        (if (eqv? (cadr env) s) (caddr env)
        (has-binding? (caddr env) s)))))
    

              

