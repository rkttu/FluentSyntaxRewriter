using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Concurrent;

namespace FluentSyntaxRewriter
{
    /// <summary>
    /// A fluent syntax rewriter for C# syntax nodes.
    /// </summary>
    public class FluentCSharpSyntaxRewriter : CSharpSyntaxRewriter
    {
        /// <summary>
        /// Start a new fluent syntax rewriter definition.
        /// </summary>
        /// <param name="visitIntoStructuredTrivia">
        /// True to visit into structured trivia; otherwise, false.
        /// </param>
        /// <returns>
        /// A new fluent syntax rewriter definition.
        /// </returns>
        public static FluentCSharpSyntaxRewriter Define(bool visitIntoStructuredTrivia = false)
            => new FluentCSharpSyntaxRewriter(visitIntoStructuredTrivia);

        /// <summary>
        /// Create a new fluent syntax rewriter.
        /// </summary>
        /// <param name="visitIntoStructuredTrivia">
        /// True to visit into structured trivia; otherwise, false.
        /// </param>
        public FluentCSharpSyntaxRewriter(bool visitIntoStructuredTrivia = false)
            : base(visitIntoStructuredTrivia) { }

        private readonly ConcurrentDictionary<Type, Delegate> _visitListElementNode = new ConcurrentDictionary<Type, Delegate>();
        private readonly ConcurrentDictionary<Type, Delegate> _visitListSeparatedList = new ConcurrentDictionary<Type, Delegate>();
        private readonly ConcurrentDictionary<Type, Delegate> _visitListNode = new ConcurrentDictionary<Type, Delegate>();

        private Func<CSharpSyntaxRewriter, SyntaxNode, SyntaxNode> _defaultVisit = default;
        private Func<CSharpSyntaxRewriter, SyntaxNode, SyntaxNode> _visit = default;
        private Func<CSharpSyntaxRewriter, SyntaxTokenList, SyntaxTokenList> _visitTokenList = default;
        private Func<CSharpSyntaxRewriter, SyntaxTriviaList, SyntaxTriviaList> _visitTriviaList = default;
        private Func<CSharpSyntaxRewriter, SyntaxTrivia, SyntaxTrivia> _visitListElement = default;
        private Func<CSharpSyntaxRewriter, SyntaxToken, SyntaxToken> _visitListSeparator = default;
        private Func<CSharpSyntaxRewriter, SyntaxToken, SyntaxToken> _visitToken = default;
        private Func<CSharpSyntaxRewriter, SyntaxTrivia, SyntaxTrivia> _visitTrivia = default;
        private Func<CSharpSyntaxRewriter, IdentifierNameSyntax, SyntaxNode> _visitIdentifierName = default;
        private Func<CSharpSyntaxRewriter, QualifiedNameSyntax, SyntaxNode> _visitQualifiedName = default;
        private Func<CSharpSyntaxRewriter, GenericNameSyntax, SyntaxNode> _visitGenericName = default;
        private Func<CSharpSyntaxRewriter, TypeArgumentListSyntax, SyntaxNode> _visitTypeArgumentList = default;
        private Func<CSharpSyntaxRewriter, AliasQualifiedNameSyntax, SyntaxNode> _visitAliasQualifiedName = default;
        private Func<CSharpSyntaxRewriter, PredefinedTypeSyntax, SyntaxNode> _visitPredefinedType = default;
        private Func<CSharpSyntaxRewriter, ArrayTypeSyntax, SyntaxNode> _visitArrayType = default;
        private Func<CSharpSyntaxRewriter, ArrayRankSpecifierSyntax, SyntaxNode> _visitArrayRankSpecifier = default;
        private Func<CSharpSyntaxRewriter, PointerTypeSyntax, SyntaxNode> _visitPointerType = default;
        private Func<CSharpSyntaxRewriter, FunctionPointerTypeSyntax, SyntaxNode> _visitFunctionPointerType = default;
        private Func<CSharpSyntaxRewriter, FunctionPointerParameterListSyntax, SyntaxNode> _visitFunctionPointerParameterList = default;
        private Func<CSharpSyntaxRewriter, FunctionPointerCallingConventionSyntax, SyntaxNode> _visitFunctionPointerCallingConvention = default;
        private Func<CSharpSyntaxRewriter, FunctionPointerUnmanagedCallingConventionListSyntax, SyntaxNode> _visitFunctionPointerUnmanagedCallingConventionList = default;
        private Func<CSharpSyntaxRewriter, FunctionPointerUnmanagedCallingConventionSyntax, SyntaxNode> _visitFunctionPointerUnmanagedCallingConvention = default;
        private Func<CSharpSyntaxRewriter, NullableTypeSyntax, SyntaxNode> _visitNullableType = default;
        private Func<CSharpSyntaxRewriter, TupleTypeSyntax, SyntaxNode> _visitTupleType = default;
        private Func<CSharpSyntaxRewriter, TupleElementSyntax, SyntaxNode> _visitTupleElement = default;
        private Func<CSharpSyntaxRewriter, OmittedTypeArgumentSyntax, SyntaxNode> _visitOmittedTypeArgument = default;
        private Func<CSharpSyntaxRewriter, RefTypeSyntax, SyntaxNode> _visitRefType = default;
        private Func<CSharpSyntaxRewriter, ScopedTypeSyntax, SyntaxNode> _visitScopedType = default;
        private Func<CSharpSyntaxRewriter, ParenthesizedExpressionSyntax, SyntaxNode> _visitParenthesizedExpression = default;
        private Func<CSharpSyntaxRewriter, TupleExpressionSyntax, SyntaxNode> _visitTupleExpression = default;
        private Func<CSharpSyntaxRewriter, PrefixUnaryExpressionSyntax, SyntaxNode> _visitPrefixUnaryExpression = default;
        private Func<CSharpSyntaxRewriter, AwaitExpressionSyntax, SyntaxNode> _visitAwaitExpression = default;
        private Func<CSharpSyntaxRewriter, PostfixUnaryExpressionSyntax, SyntaxNode> _visitPostfixUnaryExpression = default;
        private Func<CSharpSyntaxRewriter, MemberAccessExpressionSyntax, SyntaxNode> _visitMemberAccessExpression = default;
        private Func<CSharpSyntaxRewriter, ConditionalAccessExpressionSyntax, SyntaxNode> _visitConditionalAccessExpression = default;
        private Func<CSharpSyntaxRewriter, MemberBindingExpressionSyntax, SyntaxNode> _visitMemberBindingExpression = default;
        private Func<CSharpSyntaxRewriter, ElementBindingExpressionSyntax, SyntaxNode> _visitElementBindingExpression = default;
        private Func<CSharpSyntaxRewriter, RangeExpressionSyntax, SyntaxNode> _visitRangeExpression = default;
        private Func<CSharpSyntaxRewriter, ImplicitElementAccessSyntax, SyntaxNode> _visitImplicitElementAccess = default;
        private Func<CSharpSyntaxRewriter, BinaryExpressionSyntax, SyntaxNode> _visitBinaryExpression = default;
        private Func<CSharpSyntaxRewriter, AssignmentExpressionSyntax, SyntaxNode> _visitAssignmentExpression = default;
        private Func<CSharpSyntaxRewriter, ConditionalExpressionSyntax, SyntaxNode> _visitConditionalExpression = default;
        private Func<CSharpSyntaxRewriter, ThisExpressionSyntax, SyntaxNode> _visitThisExpression = default;
        private Func<CSharpSyntaxRewriter, BaseExpressionSyntax, SyntaxNode> _visitBaseExpression = default;
        private Func<CSharpSyntaxRewriter, LiteralExpressionSyntax, SyntaxNode> _visitLiteralExpression = default;
        private Func<CSharpSyntaxRewriter, MakeRefExpressionSyntax, SyntaxNode> _visitMakeRefExpression = default;
        private Func<CSharpSyntaxRewriter, RefTypeExpressionSyntax, SyntaxNode> _visitRefTypeExpression = default;
        private Func<CSharpSyntaxRewriter, RefValueExpressionSyntax, SyntaxNode> _visitRefValueExpression = default;
        private Func<CSharpSyntaxRewriter, CheckedExpressionSyntax, SyntaxNode> _visitCheckedExpression = default;
        private Func<CSharpSyntaxRewriter, DefaultExpressionSyntax, SyntaxNode> _visitDefaultExpression = default;
        private Func<CSharpSyntaxRewriter, TypeOfExpressionSyntax, SyntaxNode> _visitTypeOfExpression = default;
        private Func<CSharpSyntaxRewriter, SizeOfExpressionSyntax, SyntaxNode> _visitSizeOfExpression = default;
        private Func<CSharpSyntaxRewriter, InvocationExpressionSyntax, SyntaxNode> _visitInvocationExpression = default;
        private Func<CSharpSyntaxRewriter, ElementAccessExpressionSyntax, SyntaxNode> _visitElementAccessExpression = default;
        private Func<CSharpSyntaxRewriter, ArgumentListSyntax, SyntaxNode> _visitArgumentList = default;
        private Func<CSharpSyntaxRewriter, BracketedArgumentListSyntax, SyntaxNode> _visitBracketedArgumentList = default;
        private Func<CSharpSyntaxRewriter, ArgumentSyntax, SyntaxNode> _visitArgument = default;
        private Func<CSharpSyntaxRewriter, ExpressionColonSyntax, SyntaxNode> _visitExpressionColon = default;
        private Func<CSharpSyntaxRewriter, NameColonSyntax, SyntaxNode> _visitNameColon = default;
        private Func<CSharpSyntaxRewriter, DeclarationExpressionSyntax, SyntaxNode> _visitDeclarationExpression = default;
        private Func<CSharpSyntaxRewriter, CastExpressionSyntax, SyntaxNode> _visitCastExpression = default;
        private Func<CSharpSyntaxRewriter, AnonymousMethodExpressionSyntax, SyntaxNode> _visitAnonymousMethodExpression = default;
        private Func<CSharpSyntaxRewriter, SimpleLambdaExpressionSyntax, SyntaxNode> _visitSimpleLambdaExpression = default;
        private Func<CSharpSyntaxRewriter, RefExpressionSyntax, SyntaxNode> _visitRefExpression = default;
        private Func<CSharpSyntaxRewriter, ParenthesizedLambdaExpressionSyntax, SyntaxNode> _visitParenthesizedLambdaExpression = default;
        private Func<CSharpSyntaxRewriter, InitializerExpressionSyntax, SyntaxNode> _visitInitializerExpression = default;
        private Func<CSharpSyntaxRewriter, ImplicitObjectCreationExpressionSyntax, SyntaxNode> _visitImplicitObjectCreationExpression = default;
        private Func<CSharpSyntaxRewriter, ObjectCreationExpressionSyntax, SyntaxNode> _visitObjectCreationExpression = default;
        private Func<CSharpSyntaxRewriter, WithExpressionSyntax, SyntaxNode> _visitWithExpression = default;
        private Func<CSharpSyntaxRewriter, AnonymousObjectMemberDeclaratorSyntax, SyntaxNode> _visitAnonymousObjectMemberDeclarator = default;
        private Func<CSharpSyntaxRewriter, AnonymousObjectCreationExpressionSyntax, SyntaxNode> _visitAnonymousObjectCreationExpression = default;
        private Func<CSharpSyntaxRewriter, ArrayCreationExpressionSyntax, SyntaxNode> _visitArrayCreationExpression = default;
        private Func<CSharpSyntaxRewriter, ImplicitArrayCreationExpressionSyntax, SyntaxNode> _visitImplicitArrayCreationExpression = default;
        private Func<CSharpSyntaxRewriter, StackAllocArrayCreationExpressionSyntax, SyntaxNode> _visitStackAllocArrayCreationExpression = default;
        private Func<CSharpSyntaxRewriter, ImplicitStackAllocArrayCreationExpressionSyntax, SyntaxNode> _visitImplicitStackAllocArrayCreationExpression = default;
        private Func<CSharpSyntaxRewriter, CollectionExpressionSyntax, SyntaxNode> _visitCollectionExpression = default;
        private Func<CSharpSyntaxRewriter, ExpressionElementSyntax, SyntaxNode> _visitExpressionElement = default;
        private Func<CSharpSyntaxRewriter, SpreadElementSyntax, SyntaxNode> _visitSpreadElement = default;
        private Func<CSharpSyntaxRewriter, QueryExpressionSyntax, SyntaxNode> _visitQueryExpression = default;
        private Func<CSharpSyntaxRewriter, QueryBodySyntax, SyntaxNode> _visitQueryBody = default;
        private Func<CSharpSyntaxRewriter, FromClauseSyntax, SyntaxNode> _visitFromClause = default;
        private Func<CSharpSyntaxRewriter, LetClauseSyntax, SyntaxNode> _visitLetClause = default;
        private Func<CSharpSyntaxRewriter, JoinClauseSyntax, SyntaxNode> _visitJoinClause = default;
        private Func<CSharpSyntaxRewriter, JoinIntoClauseSyntax, SyntaxNode> _visitJoinIntoClause = default;
        private Func<CSharpSyntaxRewriter, WhereClauseSyntax, SyntaxNode> _visitWhereClause = default;
        private Func<CSharpSyntaxRewriter, OrderByClauseSyntax, SyntaxNode> _visitOrderByClause = default;
        private Func<CSharpSyntaxRewriter, OrderingSyntax, SyntaxNode> _visitOrdering = default;
        private Func<CSharpSyntaxRewriter, SelectClauseSyntax, SyntaxNode> _visitSelectClause = default;
        private Func<CSharpSyntaxRewriter, GroupClauseSyntax, SyntaxNode> _visitGroupClause = default;
        private Func<CSharpSyntaxRewriter, QueryContinuationSyntax, SyntaxNode> _visitQueryContinuation = default;
        private Func<CSharpSyntaxRewriter, OmittedArraySizeExpressionSyntax, SyntaxNode> _visitOmittedArraySizeExpression = default;
        private Func<CSharpSyntaxRewriter, InterpolatedStringExpressionSyntax, SyntaxNode> _visitInterpolatedStringExpression = default;
        private Func<CSharpSyntaxRewriter, IsPatternExpressionSyntax, SyntaxNode> _visitIsPatternExpression = default;
        private Func<CSharpSyntaxRewriter, ThrowExpressionSyntax, SyntaxNode> _visitThrowExpression = default;
        private Func<CSharpSyntaxRewriter, WhenClauseSyntax, SyntaxNode> _visitWhenClause = default;
        private Func<CSharpSyntaxRewriter, DiscardPatternSyntax, SyntaxNode> _visitDiscardPattern = default;
        private Func<CSharpSyntaxRewriter, DeclarationPatternSyntax, SyntaxNode> _visitDeclarationPattern = default;
        private Func<CSharpSyntaxRewriter, VarPatternSyntax, SyntaxNode> _visitVarPattern = default;
        private Func<CSharpSyntaxRewriter, RecursivePatternSyntax, SyntaxNode> _visitRecursivePattern = default;
        private Func<CSharpSyntaxRewriter, PositionalPatternClauseSyntax, SyntaxNode> _visitPositionalPatternClause = default;
        private Func<CSharpSyntaxRewriter, PropertyPatternClauseSyntax, SyntaxNode> _visitPropertyPatternClause = default;
        private Func<CSharpSyntaxRewriter, SubpatternSyntax, SyntaxNode> _visitSubpattern = default;
        private Func<CSharpSyntaxRewriter, ConstantPatternSyntax, SyntaxNode> _visitConstantPattern = default;
        private Func<CSharpSyntaxRewriter, ParenthesizedPatternSyntax, SyntaxNode> _visitParenthesizedPattern = default;
        private Func<CSharpSyntaxRewriter, RelationalPatternSyntax, SyntaxNode> _visitRelationalPattern = default;
        private Func<CSharpSyntaxRewriter, TypePatternSyntax, SyntaxNode> _visitTypePattern = default;
        private Func<CSharpSyntaxRewriter, BinaryPatternSyntax, SyntaxNode> _visitBinaryPattern = default;
        private Func<CSharpSyntaxRewriter, UnaryPatternSyntax, SyntaxNode> _visitUnaryPattern = default;
        private Func<CSharpSyntaxRewriter, ListPatternSyntax, SyntaxNode> _visitListPattern = default;
        private Func<CSharpSyntaxRewriter, SlicePatternSyntax, SyntaxNode> _visitSlicePattern = default;
        private Func<CSharpSyntaxRewriter, InterpolatedStringTextSyntax, SyntaxNode> _visitInterpolatedStringText = default;
        private Func<CSharpSyntaxRewriter, InterpolationSyntax, SyntaxNode> _visitInterpolation = default;
        private Func<CSharpSyntaxRewriter, InterpolationAlignmentClauseSyntax, SyntaxNode> _visitInterpolationAlignmentClause = default;
        private Func<CSharpSyntaxRewriter, InterpolationFormatClauseSyntax, SyntaxNode> _visitInterpolationFormatClause = default;
        private Func<CSharpSyntaxRewriter, GlobalStatementSyntax, SyntaxNode> _visitGlobalStatement = default;
        private Func<CSharpSyntaxRewriter, BlockSyntax, SyntaxNode> _visitBlock = default;
        private Func<CSharpSyntaxRewriter, LocalFunctionStatementSyntax, SyntaxNode> _visitLocalFunctionStatement = default;
        private Func<CSharpSyntaxRewriter, LocalDeclarationStatementSyntax, SyntaxNode> _visitLocalDeclarationStatement = default;
        private Func<CSharpSyntaxRewriter, VariableDeclarationSyntax, SyntaxNode> _visitVariableDeclaration = default;
        private Func<CSharpSyntaxRewriter, VariableDeclaratorSyntax, SyntaxNode> _visitVariableDeclarator = default;
        private Func<CSharpSyntaxRewriter, EqualsValueClauseSyntax, SyntaxNode> _visitEqualsValueClause = default;
        private Func<CSharpSyntaxRewriter, SingleVariableDesignationSyntax, SyntaxNode> _visitSingleVariableDesignation = default;
        private Func<CSharpSyntaxRewriter, DiscardDesignationSyntax, SyntaxNode> _visitDiscardDesignation = default;
        private Func<CSharpSyntaxRewriter, ParenthesizedVariableDesignationSyntax, SyntaxNode> _visitParenthesizedVariableDesignation = default;
        private Func<CSharpSyntaxRewriter, ExpressionStatementSyntax, SyntaxNode> _visitExpressionStatement = default;
        private Func<CSharpSyntaxRewriter, EmptyStatementSyntax, SyntaxNode> _visitEmptyStatement = default;
        private Func<CSharpSyntaxRewriter, LabeledStatementSyntax, SyntaxNode> _visitLabeledStatement = default;
        private Func<CSharpSyntaxRewriter, GotoStatementSyntax, SyntaxNode> _visitGotoStatement = default;
        private Func<CSharpSyntaxRewriter, BreakStatementSyntax, SyntaxNode> _visitBreakStatement = default;
        private Func<CSharpSyntaxRewriter, ContinueStatementSyntax, SyntaxNode> _visitContinueStatement = default;
        private Func<CSharpSyntaxRewriter, ReturnStatementSyntax, SyntaxNode> _visitReturnStatement = default;
        private Func<CSharpSyntaxRewriter, ThrowStatementSyntax, SyntaxNode> _visitThrowStatement = default;
        private Func<CSharpSyntaxRewriter, YieldStatementSyntax, SyntaxNode> _visitYieldStatement = default;
        private Func<CSharpSyntaxRewriter, WhileStatementSyntax, SyntaxNode> _visitWhileStatement = default;
        private Func<CSharpSyntaxRewriter, DoStatementSyntax, SyntaxNode> _visitDoStatement = default;
        private Func<CSharpSyntaxRewriter, ForStatementSyntax, SyntaxNode> _visitForStatement = default;
        private Func<CSharpSyntaxRewriter, ForEachStatementSyntax, SyntaxNode> _visitForEachStatement = default;
        private Func<CSharpSyntaxRewriter, ForEachVariableStatementSyntax, SyntaxNode> _visitForEachVariableStatement = default;
        private Func<CSharpSyntaxRewriter, UsingStatementSyntax, SyntaxNode> _visitUsingStatement = default;
        private Func<CSharpSyntaxRewriter, FixedStatementSyntax, SyntaxNode> _visitFixedStatement = default;
        private Func<CSharpSyntaxRewriter, CheckedStatementSyntax, SyntaxNode> _visitCheckedStatement = default;
        private Func<CSharpSyntaxRewriter, UnsafeStatementSyntax, SyntaxNode> _visitUnsafeStatement = default;
        private Func<CSharpSyntaxRewriter, LockStatementSyntax, SyntaxNode> _visitLockStatement = default;
        private Func<CSharpSyntaxRewriter, IfStatementSyntax, SyntaxNode> _visitIfStatement = default;
        private Func<CSharpSyntaxRewriter, ElseClauseSyntax, SyntaxNode> _visitElseClause = default;
        private Func<CSharpSyntaxRewriter, SwitchStatementSyntax, SyntaxNode> _visitSwitchStatement = default;
        private Func<CSharpSyntaxRewriter, SwitchSectionSyntax, SyntaxNode> _visitSwitchSection = default;
        private Func<CSharpSyntaxRewriter, CasePatternSwitchLabelSyntax, SyntaxNode> _visitCasePatternSwitchLabel = default;
        private Func<CSharpSyntaxRewriter, CaseSwitchLabelSyntax, SyntaxNode> _visitCaseSwitchLabel = default;
        private Func<CSharpSyntaxRewriter, DefaultSwitchLabelSyntax, SyntaxNode> _visitDefaultSwitchLabel = default;
        private Func<CSharpSyntaxRewriter, SwitchExpressionSyntax, SyntaxNode> _visitSwitchExpression = default;
        private Func<CSharpSyntaxRewriter, SwitchExpressionArmSyntax, SyntaxNode> _visitSwitchExpressionArm = default;
        private Func<CSharpSyntaxRewriter, TryStatementSyntax, SyntaxNode> _visitTryStatement = default;
        private Func<CSharpSyntaxRewriter, CatchClauseSyntax, SyntaxNode> _visitCatchClause = default;
        private Func<CSharpSyntaxRewriter, CatchDeclarationSyntax, SyntaxNode> _visitCatchDeclaration = default;
        private Func<CSharpSyntaxRewriter, CatchFilterClauseSyntax, SyntaxNode> _visitCatchFilterClause = default;
        private Func<CSharpSyntaxRewriter, FinallyClauseSyntax, SyntaxNode> _visitFinallyClause = default;
        private Func<CSharpSyntaxRewriter, CompilationUnitSyntax, SyntaxNode> _visitCompilationUnit = default;
        private Func<CSharpSyntaxRewriter, ExternAliasDirectiveSyntax, SyntaxNode> _visitExternAliasDirective = default;
        private Func<CSharpSyntaxRewriter, UsingDirectiveSyntax, SyntaxNode> _visitUsingDirective = default;
        private Func<CSharpSyntaxRewriter, NamespaceDeclarationSyntax, SyntaxNode> _visitNamespaceDeclaration = default;
        private Func<CSharpSyntaxRewriter, FileScopedNamespaceDeclarationSyntax, SyntaxNode> _visitFileScopedNamespaceDeclaration = default;
        private Func<CSharpSyntaxRewriter, AttributeListSyntax, SyntaxNode> _visitAttributeList = default;
        private Func<CSharpSyntaxRewriter, AttributeTargetSpecifierSyntax, SyntaxNode> _visitAttributeTargetSpecifier = default;
        private Func<CSharpSyntaxRewriter, AttributeSyntax, SyntaxNode> _visitAttribute = default;
        private Func<CSharpSyntaxRewriter, AttributeArgumentListSyntax, SyntaxNode> _visitAttributeArgumentList = default;
        private Func<CSharpSyntaxRewriter, AttributeArgumentSyntax, SyntaxNode> _visitAttributeArgument = default;
        private Func<CSharpSyntaxRewriter, NameEqualsSyntax, SyntaxNode> _visitNameEquals = default;
        private Func<CSharpSyntaxRewriter, TypeParameterListSyntax, SyntaxNode> _visitTypeParameterList = default;
        private Func<CSharpSyntaxRewriter, TypeParameterSyntax, SyntaxNode> _visitTypeParameter = default;
        private Func<CSharpSyntaxRewriter, ClassDeclarationSyntax, SyntaxNode> _visitClassDeclaration = default;
        private Func<CSharpSyntaxRewriter, StructDeclarationSyntax, SyntaxNode> _visitStructDeclaration = default;
        private Func<CSharpSyntaxRewriter, InterfaceDeclarationSyntax, SyntaxNode> _visitInterfaceDeclaration = default;
        private Func<CSharpSyntaxRewriter, RecordDeclarationSyntax, SyntaxNode> _visitRecordDeclaration = default;
        private Func<CSharpSyntaxRewriter, EnumDeclarationSyntax, SyntaxNode> _visitEnumDeclaration = default;
        private Func<CSharpSyntaxRewriter, DelegateDeclarationSyntax, SyntaxNode> _visitDelegateDeclaration = default;
        private Func<CSharpSyntaxRewriter, EnumMemberDeclarationSyntax, SyntaxNode> _visitEnumMemberDeclaration = default;
        private Func<CSharpSyntaxRewriter, BaseListSyntax, SyntaxNode> _visitBaseList = default;
        private Func<CSharpSyntaxRewriter, SimpleBaseTypeSyntax, SyntaxNode> _visitSimpleBaseType = default;
        private Func<CSharpSyntaxRewriter, PrimaryConstructorBaseTypeSyntax, SyntaxNode> _visitPrimaryConstructorBaseType = default;
        private Func<CSharpSyntaxRewriter, TypeParameterConstraintClauseSyntax, SyntaxNode> _visitTypeParameterConstraintClause = default;
        private Func<CSharpSyntaxRewriter, ConstructorConstraintSyntax, SyntaxNode> _visitConstructorConstraint = default;
        private Func<CSharpSyntaxRewriter, ClassOrStructConstraintSyntax, SyntaxNode> _visitClassOrStructConstraint = default;
        private Func<CSharpSyntaxRewriter, TypeConstraintSyntax, SyntaxNode> _visitTypeConstraint = default;
        private Func<CSharpSyntaxRewriter, DefaultConstraintSyntax, SyntaxNode> _visitDefaultConstraint = default;
        private Func<CSharpSyntaxRewriter, FieldDeclarationSyntax, SyntaxNode> _visitFieldDeclaration = default;
        private Func<CSharpSyntaxRewriter, EventFieldDeclarationSyntax, SyntaxNode> _visitEventFieldDeclaration = default;
        private Func<CSharpSyntaxRewriter, ExplicitInterfaceSpecifierSyntax, SyntaxNode> _visitExplicitInterfaceSpecifier = default;
        private Func<CSharpSyntaxRewriter, MethodDeclarationSyntax, SyntaxNode> _visitMethodDeclaration = default;
        private Func<CSharpSyntaxRewriter, OperatorDeclarationSyntax, SyntaxNode> _visitOperatorDeclaration = default;
        private Func<CSharpSyntaxRewriter, ConversionOperatorDeclarationSyntax, SyntaxNode> _visitConversionOperatorDeclaration = default;
        private Func<CSharpSyntaxRewriter, ConstructorDeclarationSyntax, SyntaxNode> _visitConstructorDeclaration = default;
        private Func<CSharpSyntaxRewriter, ConstructorInitializerSyntax, SyntaxNode> _visitConstructorInitializer = default;
        private Func<CSharpSyntaxRewriter, DestructorDeclarationSyntax, SyntaxNode> _visitDestructorDeclaration = default;
        private Func<CSharpSyntaxRewriter, PropertyDeclarationSyntax, SyntaxNode> _visitPropertyDeclaration = default;
        private Func<CSharpSyntaxRewriter, ArrowExpressionClauseSyntax, SyntaxNode> _visitArrowExpressionClause = default;
        private Func<CSharpSyntaxRewriter, EventDeclarationSyntax, SyntaxNode> _visitEventDeclaration = default;
        private Func<CSharpSyntaxRewriter, IndexerDeclarationSyntax, SyntaxNode> _visitIndexerDeclaration = default;
        private Func<CSharpSyntaxRewriter, AccessorListSyntax, SyntaxNode> _visitAccessorList = default;
        private Func<CSharpSyntaxRewriter, AccessorDeclarationSyntax, SyntaxNode> _visitAccessorDeclaration = default;
        private Func<CSharpSyntaxRewriter, ParameterListSyntax, SyntaxNode> _visitParameterList = default;
        private Func<CSharpSyntaxRewriter, BracketedParameterListSyntax, SyntaxNode> _visitBracketedParameterList = default;
        private Func<CSharpSyntaxRewriter, ParameterSyntax, SyntaxNode> _visitParameter = default;
        private Func<CSharpSyntaxRewriter, FunctionPointerParameterSyntax, SyntaxNode> _visitFunctionPointerParameter = default;
        private Func<CSharpSyntaxRewriter, IncompleteMemberSyntax, SyntaxNode> _visitIncompleteMember = default;
        private Func<CSharpSyntaxRewriter, SkippedTokensTriviaSyntax, SyntaxNode> _visitSkippedTokensTrivia = default;
        private Func<CSharpSyntaxRewriter, DocumentationCommentTriviaSyntax, SyntaxNode> _visitDocumentationCommentTrivia = default;
        private Func<CSharpSyntaxRewriter, TypeCrefSyntax, SyntaxNode> _visitTypeCref = default;
        private Func<CSharpSyntaxRewriter, QualifiedCrefSyntax, SyntaxNode> _visitQualifiedCref = default;
        private Func<CSharpSyntaxRewriter, NameMemberCrefSyntax, SyntaxNode> _visitNameMemberCref = default;
        private Func<CSharpSyntaxRewriter, IndexerMemberCrefSyntax, SyntaxNode> _visitIndexerMemberCref = default;
        private Func<CSharpSyntaxRewriter, OperatorMemberCrefSyntax, SyntaxNode> _visitOperatorMemberCref = default;
        private Func<CSharpSyntaxRewriter, ConversionOperatorMemberCrefSyntax, SyntaxNode> _visitConversionOperatorMemberCref = default;
        private Func<CSharpSyntaxRewriter, CrefParameterListSyntax, SyntaxNode> _visitCrefParameterList = default;
        private Func<CSharpSyntaxRewriter, CrefBracketedParameterListSyntax, SyntaxNode> _visitCrefBracketedParameterList = default;
        private Func<CSharpSyntaxRewriter, CrefParameterSyntax, SyntaxNode> _visitCrefParameter = default;
        private Func<CSharpSyntaxRewriter, XmlElementSyntax, SyntaxNode> _visitXmlElement = default;
        private Func<CSharpSyntaxRewriter, XmlElementStartTagSyntax, SyntaxNode> _visitXmlElementStartTag = default;
        private Func<CSharpSyntaxRewriter, XmlElementEndTagSyntax, SyntaxNode> _visitXmlElementEndTag = default;
        private Func<CSharpSyntaxRewriter, XmlEmptyElementSyntax, SyntaxNode> _visitXmlEmptyElement = default;
        private Func<CSharpSyntaxRewriter, XmlNameSyntax, SyntaxNode> _visitXmlName = default;
        private Func<CSharpSyntaxRewriter, XmlPrefixSyntax, SyntaxNode> _visitXmlPrefix = default;
        private Func<CSharpSyntaxRewriter, XmlTextAttributeSyntax, SyntaxNode> _visitXmlTextAttribute = default;
        private Func<CSharpSyntaxRewriter, XmlCrefAttributeSyntax, SyntaxNode> _visitXmlCrefAttribute = default;
        private Func<CSharpSyntaxRewriter, XmlNameAttributeSyntax, SyntaxNode> _visitXmlNameAttribute = default;
        private Func<CSharpSyntaxRewriter, XmlTextSyntax, SyntaxNode> _visitXmlText = default;
        private Func<CSharpSyntaxRewriter, XmlCDataSectionSyntax, SyntaxNode> _visitXmlCDataSection = default;
        private Func<CSharpSyntaxRewriter, XmlProcessingInstructionSyntax, SyntaxNode> _visitXmlProcessingInstruction = default;
        private Func<CSharpSyntaxRewriter, XmlCommentSyntax, SyntaxNode> _visitXmlComment = default;
        private Func<CSharpSyntaxRewriter, IfDirectiveTriviaSyntax, SyntaxNode> _visitIfDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, ElifDirectiveTriviaSyntax, SyntaxNode> _visitElifDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, ElseDirectiveTriviaSyntax, SyntaxNode> _visitElseDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, EndIfDirectiveTriviaSyntax, SyntaxNode> _visitEndIfDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, RegionDirectiveTriviaSyntax, SyntaxNode> _visitRegionDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, EndRegionDirectiveTriviaSyntax, SyntaxNode> _visitEndRegionDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, ErrorDirectiveTriviaSyntax, SyntaxNode> _visitErrorDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, WarningDirectiveTriviaSyntax, SyntaxNode> _visitWarningDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, BadDirectiveTriviaSyntax, SyntaxNode> _visitBadDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, DefineDirectiveTriviaSyntax, SyntaxNode> _visitDefineDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, UndefDirectiveTriviaSyntax, SyntaxNode> _visitUndefDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, LineDirectiveTriviaSyntax, SyntaxNode> _visitLineDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, LineDirectivePositionSyntax, SyntaxNode> _visitLineDirectivePosition = default;
        private Func<CSharpSyntaxRewriter, LineSpanDirectiveTriviaSyntax, SyntaxNode> _visitLineSpanDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, PragmaWarningDirectiveTriviaSyntax, SyntaxNode> _visitPragmaWarningDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, PragmaChecksumDirectiveTriviaSyntax, SyntaxNode> _visitPragmaChecksumDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, ReferenceDirectiveTriviaSyntax, SyntaxNode> _visitReferenceDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, LoadDirectiveTriviaSyntax, SyntaxNode> _visitLoadDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, ShebangDirectiveTriviaSyntax, SyntaxNode> _visitShebangDirectiveTrivia = default;
        private Func<CSharpSyntaxRewriter, NullableDirectiveTriviaSyntax, SyntaxNode> _visitNullableDirectiveTrivia = default;

        /// <summary>
        /// Set the default visit method.
        /// </summary>
        /// <param name="defaultVisit">
        /// The method to use; or null to use the default method.
        /// </param>
        /// <returns>
        /// The fluent syntax rewriter instance itself.
        /// </returns>
        public virtual FluentCSharpSyntaxRewriter WithDefaultVisit(Func<CSharpSyntaxRewriter, SyntaxNode, SyntaxNode> defaultVisit = default)
        {
            _defaultVisit = defaultVisit;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during node visits.
        /// </summary>
        /// <param name="visit">The function to call during node visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisit(Func<CSharpSyntaxRewriter, SyntaxNode, SyntaxNode> visit = default)
        {
            _visit = visit;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during token list visits.
        /// </summary>
        /// <param name="visitTokenList">The function to call during token list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitTokenList(Func<CSharpSyntaxRewriter, SyntaxTokenList, SyntaxTokenList> visitTokenList = default)
        {
            _visitTokenList = visitTokenList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during trivia list visits.
        /// </summary>
        /// <param name="visitTriviaList">The function to call during trivia list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitTriviaList(Func<CSharpSyntaxRewriter, SyntaxTriviaList, SyntaxTriviaList> visitTriviaList = default)
        {
            _visitTriviaList = visitTriviaList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during list element visits.
        /// </summary>
        /// <param name="visitListElement">The function to call during list element visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitListElement(Func<CSharpSyntaxRewriter, SyntaxTrivia, SyntaxTrivia> visitListElement = default)
        {
            _visitListElement = visitListElement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during list separator visits.
        /// </summary>
        /// <param name="visitListSeparator">The function to call during list separator visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitListSeparator(Func<CSharpSyntaxRewriter, SyntaxToken, SyntaxToken> visitListSeparator = default)
        {
            _visitListSeparator = visitListSeparator;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during list element node visits.
        /// </summary>
        /// <typeparam name="TNode">
        /// The type of the node to visit.
        /// </typeparam>
        /// <param name="visitListElementNode">The function to call during list element node visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitListElementNode<TNode>(Func<CSharpSyntaxRewriter, SyntaxNode, SyntaxNode> visitListElementNode = default)
            where TNode : SyntaxNode
        {
            _visitListElementNode.AddOrUpdate(
                typeof(TNode),
                visitListElementNode,
                (_, __) => visitListElementNode);
            return this;
        }

        /// <summary>
        /// Sets a function to be called during separated list visits.
        /// </summary>
        /// <typeparam name="TNode">
        /// The type of the node to visit.
        /// </typeparam>
        /// <param name="visitListSeparatedList">The function to call during separated list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitListSeparatedList<TNode>(Func<CSharpSyntaxRewriter, SeparatedSyntaxList<SyntaxNode>, SeparatedSyntaxList<SyntaxNode>> visitListSeparatedList = default)
            where TNode : SyntaxNode
        {
            _visitListSeparatedList.AddOrUpdate(
                typeof(TNode),
                visitListSeparatedList,
                (_, __) => visitListSeparatedList);
            return this;
        }

        /// <summary>
        /// Sets a function to be called during list node visits.
        /// </summary>
        /// <typeparam name="TNode">
        /// The type of the node to visit.
        /// </typeparam>
        /// <param name="visitListNode">The function to call during list node visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitListNode<TNode>(Func<CSharpSyntaxRewriter, SyntaxList<SyntaxNode>, SyntaxList<SyntaxNode>> visitListNode = default)
            where TNode : SyntaxNode
        {
            _visitListNode.AddOrUpdate(
                typeof(TNode),
                visitListNode,
                (_, __) => visitListNode);
            return this;
        }

        /// <summary>
        /// Sets a function to be called during token visits.
        /// </summary>
        /// <param name="visitToken">The function to call during token visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitToken(Func<CSharpSyntaxRewriter, SyntaxToken, SyntaxToken> visitToken = default)
        {
            _visitToken = visitToken;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during trivia visits.
        /// </summary>
        /// <param name="visitTrivia">The function to call during trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitTrivia(Func<CSharpSyntaxRewriter, SyntaxTrivia, SyntaxTrivia> visitTrivia = default)
        {
            _visitTrivia = visitTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during identifier name visits.
        /// </summary>
        /// <param name="visitIdentifierName">The function to call during identifier name visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitIdentifierName(Func<CSharpSyntaxRewriter, IdentifierNameSyntax, SyntaxNode> visitIdentifierName = default)
        {
            _visitIdentifierName = visitIdentifierName;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during qualified name visits.
        /// </summary>
        /// <param name="visitQualifiedName">The function to call during qualified name visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitQualifiedName(Func<CSharpSyntaxRewriter, QualifiedNameSyntax, SyntaxNode> visitQualifiedName = default)
        {
            _visitQualifiedName = visitQualifiedName;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during generic name visits.
        /// </summary>
        /// <param name="visitGenericName">The function to call during generic name visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitGenericName(Func<CSharpSyntaxRewriter, GenericNameSyntax, SyntaxNode> visitGenericName = default)
        {
            _visitGenericName = visitGenericName;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during type argument list visits.
        /// </summary>
        /// <param name="visitTypeArgumentList">The function to call during type argument list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitTypeArgumentList(Func<CSharpSyntaxRewriter, TypeArgumentListSyntax, SyntaxNode> visitTypeArgumentList = default)
        {
            _visitTypeArgumentList = visitTypeArgumentList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during alias qualified name visits.
        /// </summary>
        /// <param name="visitAliasQualifiedName">The function to call during alias qualified name visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitAliasQualifiedName(Func<CSharpSyntaxRewriter, AliasQualifiedNameSyntax, SyntaxNode> visitAliasQualifiedName = default)
        {
            _visitAliasQualifiedName = visitAliasQualifiedName;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during predefined type visits.
        /// </summary>
        /// <param name="visitPredefinedType">The function to call during predefined type visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitPredefinedType(Func<CSharpSyntaxRewriter, PredefinedTypeSyntax, SyntaxNode> visitPredefinedType = default)
        {
            _visitPredefinedType = visitPredefinedType;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during array type visits.
        /// </summary>
        /// <param name="visitArrayType">The function to call during array type visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitArrayType(Func<CSharpSyntaxRewriter, ArrayTypeSyntax, SyntaxNode> visitArrayType = default)
        {
            _visitArrayType = visitArrayType;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during array rank specifier visits.
        /// </summary>
        /// <param name="visitArrayRankSpecifier">The function to call during array rank specifier visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitArrayRankSpecifier(Func<CSharpSyntaxRewriter, ArrayRankSpecifierSyntax, SyntaxNode> visitArrayRankSpecifier = default)
        {
            _visitArrayRankSpecifier = visitArrayRankSpecifier;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during pointer type visits.
        /// </summary>
        /// <param name="visitPointerType">The function to call during pointer type visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitPointerType(Func<CSharpSyntaxRewriter, PointerTypeSyntax, SyntaxNode> visitPointerType = default)
        {
            _visitPointerType = visitPointerType;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during function pointer type visits.
        /// </summary>
        /// <param name="visitFunctionPointerType">The function to call during function pointer type visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitFunctionPointerType(Func<CSharpSyntaxRewriter, FunctionPointerTypeSyntax, SyntaxNode> visitFunctionPointerType = default)
        {
            _visitFunctionPointerType = visitFunctionPointerType;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during function pointer parameter list visits.
        /// </summary>
        /// <param name="visitFunctionPointerParameterList">The function to call during function pointer parameter list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitFunctionPointerParameterList(Func<CSharpSyntaxRewriter, FunctionPointerParameterListSyntax, SyntaxNode> visitFunctionPointerParameterList = default)
        {
            _visitFunctionPointerParameterList = visitFunctionPointerParameterList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during function pointer calling convention visits.
        /// </summary>
        /// <param name="visitFunctionPointerCallingConvention">The function to call during function pointer calling convention visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitFunctionPointerCallingConvention(Func<CSharpSyntaxRewriter, FunctionPointerCallingConventionSyntax, SyntaxNode> visitFunctionPointerCallingConvention = default)
        {
            _visitFunctionPointerCallingConvention = visitFunctionPointerCallingConvention;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during function pointer unmanaged calling convention list visits.
        /// </summary>
        /// <param name="visitFunctionPointerUnmanagedCallingConventionList">The function to call during function pointer unmanaged calling convention list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitFunctionPointerUnmanagedCallingConventionList(Func<CSharpSyntaxRewriter, FunctionPointerUnmanagedCallingConventionListSyntax, SyntaxNode> visitFunctionPointerUnmanagedCallingConventionList = default)
        {
            _visitFunctionPointerUnmanagedCallingConventionList = visitFunctionPointerUnmanagedCallingConventionList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during function pointer unmanaged calling convention visits.
        /// </summary>
        /// <param name="visitFunctionPointerUnmanagedCallingConvention">The function to call during function pointer unmanaged calling convention visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitFunctionPointerUnmanagedCallingConvention(Func<CSharpSyntaxRewriter, FunctionPointerUnmanagedCallingConventionSyntax, SyntaxNode> visitFunctionPointerUnmanagedCallingConvention = default)
        {
            _visitFunctionPointerUnmanagedCallingConvention = visitFunctionPointerUnmanagedCallingConvention;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during nullable type visits.
        /// </summary>
        /// <param name="visitNullableType">The function to call during nullable type visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitNullableType(Func<CSharpSyntaxRewriter, NullableTypeSyntax, SyntaxNode> visitNullableType = default)
        {
            _visitNullableType = visitNullableType;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during tuple type visits.
        /// </summary>
        /// <param name="visitTupleType">The function to call during tuple type visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitTupleType(Func<CSharpSyntaxRewriter, TupleTypeSyntax, SyntaxNode> visitTupleType = default)
        {
            _visitTupleType = visitTupleType;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during tuple element visits.
        /// </summary>
        /// <param name="visitTupleElement">The function to call during tuple element visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitTupleElement(Func<CSharpSyntaxRewriter, TupleElementSyntax, SyntaxNode> visitTupleElement = default)
        {
            _visitTupleElement = visitTupleElement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during omitted type argument visits.
        /// </summary>
        /// <param name="visitOmittedTypeArgument">The function to call during omitted type argument visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitOmittedTypeArgument(Func<CSharpSyntaxRewriter, OmittedTypeArgumentSyntax, SyntaxNode> visitOmittedTypeArgument = default)
        {
            _visitOmittedTypeArgument = visitOmittedTypeArgument;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during ref type visits.
        /// </summary>
        /// <param name="visitRefType">The function to call during ref type visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitRefType(Func<CSharpSyntaxRewriter, RefTypeSyntax, SyntaxNode> visitRefType = default)
        {
            _visitRefType = visitRefType;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during scoped type visits.
        /// </summary>
        /// <param name="visitScopedType">The function to call during scoped type visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitScopedType(Func<CSharpSyntaxRewriter, ScopedTypeSyntax, SyntaxNode> visitScopedType = default)
        {
            _visitScopedType = visitScopedType;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during parenthesized expression visits.
        /// </summary>
        /// <param name="visitParenthesizedExpression">The function to call during parenthesized expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitParenthesizedExpression(Func<CSharpSyntaxRewriter, ParenthesizedExpressionSyntax, SyntaxNode> visitParenthesizedExpression = default)
        {
            _visitParenthesizedExpression = visitParenthesizedExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during tuple expression visits.
        /// </summary>
        /// <param name="visitTupleExpression">The function to call during tuple expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitTupleExpression(Func<CSharpSyntaxRewriter, TupleExpressionSyntax, SyntaxNode> visitTupleExpression = default)
        {
            _visitTupleExpression = visitTupleExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during prefix unary expression visits.
        /// </summary>
        /// <param name="visitPrefixUnaryExpression">The function to call during prefix unary expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitPrefixUnaryExpression(Func<CSharpSyntaxRewriter, PrefixUnaryExpressionSyntax, SyntaxNode> visitPrefixUnaryExpression = default)
        {
            _visitPrefixUnaryExpression = visitPrefixUnaryExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during await expression visits.
        /// </summary>
        /// <param name="visitAwaitExpression">The function to call during await expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitAwaitExpression(Func<CSharpSyntaxRewriter, AwaitExpressionSyntax, SyntaxNode> visitAwaitExpression = default)
        {
            _visitAwaitExpression = visitAwaitExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during postfix unary expression visits.
        /// </summary>
        /// <param name="visitPostfixUnaryExpression">The function to call during postfix unary expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitPostfixUnaryExpression(Func<CSharpSyntaxRewriter, PostfixUnaryExpressionSyntax, SyntaxNode> visitPostfixUnaryExpression = default)
        {
            _visitPostfixUnaryExpression = visitPostfixUnaryExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during member access expression visits.
        /// </summary>
        /// <param name="visitMemberAccessExpression">The function to call during member access expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitMemberAccessExpression(Func<CSharpSyntaxRewriter, MemberAccessExpressionSyntax, SyntaxNode> visitMemberAccessExpression = default)
        {
            _visitMemberAccessExpression = visitMemberAccessExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during conditional access expression visits.
        /// </summary>
        /// <param name="visitConditionalAccessExpression">The function to call during conditional access expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitConditionalAccessExpression(Func<CSharpSyntaxRewriter, ConditionalAccessExpressionSyntax, SyntaxNode> visitConditionalAccessExpression = default)
        {
            _visitConditionalAccessExpression = visitConditionalAccessExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during member binding expression visits.
        /// </summary>
        /// <param name="visitMemberBindingExpression">The function to call during member binding expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitMemberBindingExpression(Func<CSharpSyntaxRewriter, MemberBindingExpressionSyntax, SyntaxNode> visitMemberBindingExpression = default)
        {
            _visitMemberBindingExpression = visitMemberBindingExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during element binding expression visits.
        /// </summary>
        /// <param name="visitElementBindingExpression">The function to call during element binding expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitElementBindingExpression(Func<CSharpSyntaxRewriter, ElementBindingExpressionSyntax, SyntaxNode> visitElementBindingExpression = default)
        {
            _visitElementBindingExpression = visitElementBindingExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during range expression visits.
        /// </summary>
        /// <param name="visitRangeExpression">The function to call during range expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitRangeExpression(Func<CSharpSyntaxRewriter, RangeExpressionSyntax, SyntaxNode> visitRangeExpression = default)
        {
            _visitRangeExpression = visitRangeExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during implicit element access visits.
        /// </summary>
        /// <param name="visitImplicitElementAccess">The function to call during implicit element access visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitImplicitElementAccess(Func<CSharpSyntaxRewriter, ImplicitElementAccessSyntax, SyntaxNode> visitImplicitElementAccess = default)
        {
            _visitImplicitElementAccess = visitImplicitElementAccess;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during binary expression visits.
        /// </summary>
        /// <param name="visitBinaryExpression">The function to call during binary expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitBinaryExpression(Func<CSharpSyntaxRewriter, BinaryExpressionSyntax, SyntaxNode> visitBinaryExpression = default)
        {
            _visitBinaryExpression = visitBinaryExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during assignment expression visits.
        /// </summary>
        /// <param name="visitAssignmentExpression">The function to call during assignment expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitAssignmentExpression(Func<CSharpSyntaxRewriter, AssignmentExpressionSyntax, SyntaxNode> visitAssignmentExpression = default)
        {
            _visitAssignmentExpression = visitAssignmentExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during conditional expression visits.
        /// </summary>
        /// <param name="visitConditionalExpression">The function to call during conditional expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitConditionalExpression(Func<CSharpSyntaxRewriter, ConditionalExpressionSyntax, SyntaxNode> visitConditionalExpression = default)
        {
            _visitConditionalExpression = visitConditionalExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during "this" expression visits.
        /// </summary>
        /// <param name="visitThisExpression">The function to call during "this" expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitThisExpression(Func<CSharpSyntaxRewriter, ThisExpressionSyntax, SyntaxNode> visitThisExpression = default)
        {
            _visitThisExpression = visitThisExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during "base" expression visits.
        /// </summary>
        /// <param name="visitBaseExpression">The function to call during "base" expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitBaseExpression(Func<CSharpSyntaxRewriter, BaseExpressionSyntax, SyntaxNode> visitBaseExpression = default)
        {
            _visitBaseExpression = visitBaseExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during literal expression visits.
        /// </summary>
        /// <param name="visitLiteralExpression">The function to call during literal expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitLiteralExpression(Func<CSharpSyntaxRewriter, LiteralExpressionSyntax, SyntaxNode> visitLiteralExpression = default)
        {
            _visitLiteralExpression = visitLiteralExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during make ref expression visits.
        /// </summary>
        /// <param name="visitMakeRefExpression">The function to call during make ref expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitMakeRefExpression(Func<CSharpSyntaxRewriter, MakeRefExpressionSyntax, SyntaxNode> visitMakeRefExpression = default)
        {
            _visitMakeRefExpression = visitMakeRefExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during ref type expression visits.
        /// </summary>
        /// <param name="visitRefTypeExpression">The function to call during ref type expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitRefTypeExpression(Func<CSharpSyntaxRewriter, RefTypeExpressionSyntax, SyntaxNode> visitRefTypeExpression = default)
        {
            _visitRefTypeExpression = visitRefTypeExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during ref value expression visits.
        /// </summary>
        /// <param name="visitRefValueExpression">The function to call during ref value expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitRefValueExpression(Func<CSharpSyntaxRewriter, RefValueExpressionSyntax, SyntaxNode> visitRefValueExpression = default)
        {
            _visitRefValueExpression = visitRefValueExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during checked expression visits.
        /// </summary>
        /// <param name="visitCheckedExpression">The function to call during checked expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitCheckedExpression(Func<CSharpSyntaxRewriter, CheckedExpressionSyntax, SyntaxNode> visitCheckedExpression = default)
        {
            _visitCheckedExpression = visitCheckedExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during default expression visits.
        /// </summary>
        /// <param name="visitDefaultExpression">The function to call during default expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitDefaultExpression(Func<CSharpSyntaxRewriter, DefaultExpressionSyntax, SyntaxNode> visitDefaultExpression = default)
        {
            _visitDefaultExpression = visitDefaultExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during typeof expression visits.
        /// </summary>
        /// <param name="visitTypeOfExpression">The function to call during typeof expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitTypeOfExpression(Func<CSharpSyntaxRewriter, TypeOfExpressionSyntax, SyntaxNode> visitTypeOfExpression = default)
        {
            _visitTypeOfExpression = visitTypeOfExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during sizeof expression visits.
        /// </summary>
        /// <param name="visitSizeOfExpression">The function to call during sizeof expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitSizeOfExpression(Func<CSharpSyntaxRewriter, SizeOfExpressionSyntax, SyntaxNode> visitSizeOfExpression = default)
        {
            _visitSizeOfExpression = visitSizeOfExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during invocation expression visits.
        /// </summary>
        /// <param name="visitInvocationExpression">The function to call during invocation expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitInvocationExpression(Func<CSharpSyntaxRewriter, InvocationExpressionSyntax, SyntaxNode> visitInvocationExpression = default)
        {
            _visitInvocationExpression = visitInvocationExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during element access expression visits.
        /// </summary>
        /// <param name="visitElementAccessExpression">The function to call during element access expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitElementAccessExpression(Func<CSharpSyntaxRewriter, ElementAccessExpressionSyntax, SyntaxNode> visitElementAccessExpression = default)
        {
            _visitElementAccessExpression = visitElementAccessExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during argument list visits.
        /// </summary>
        /// <param name = "visitArgumentList" > The function to call during argument list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitArgumentList(Func<CSharpSyntaxRewriter, ArgumentListSyntax, SyntaxNode> visitArgumentList = default)
        {
            _visitArgumentList = visitArgumentList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during bracketed argument list visits.
        /// </summary>
        /// <param name="visitBracketedArgumentList">The function to call during bracketed argument list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitBracketedArgumentList(Func<CSharpSyntaxRewriter, BracketedArgumentListSyntax, SyntaxNode> visitBracketedArgumentList = default)
        {
            _visitBracketedArgumentList = visitBracketedArgumentList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during argument visits.
        /// </summary>
        /// <param name="visitArgument">The function to call during argument visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitArgument(Func<CSharpSyntaxRewriter, ArgumentSyntax, SyntaxNode> visitArgument = default)
        {
            _visitArgument = visitArgument;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during expression colon visits.
        /// </summary>
        /// <param name="visitExpressionColon">The function to call during expression colon visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitExpressionColon(Func<CSharpSyntaxRewriter, ExpressionColonSyntax, SyntaxNode> visitExpressionColon = default)
        {
            _visitExpressionColon = visitExpressionColon;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during name colon visits.
        /// </summary>
        /// <param name="visitNameColon">The function to call during name colon visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitNameColon(Func<CSharpSyntaxRewriter, NameColonSyntax, SyntaxNode> visitNameColon = default)
        {
            _visitNameColon = visitNameColon;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during declaration expression visits.
        /// </summary>
        /// <param name="visitDeclarationExpression">The function to call during declaration expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitDeclarationExpression(Func<CSharpSyntaxRewriter, DeclarationExpressionSyntax, SyntaxNode> visitDeclarationExpression = default)
        {
            _visitDeclarationExpression = visitDeclarationExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during cast expression visits.
        /// </summary>
        /// <param name="visitCastExpression">The function to call during cast expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitCastExpression(Func<CSharpSyntaxRewriter, CastExpressionSyntax, SyntaxNode> visitCastExpression = default)
        {
            _visitCastExpression = visitCastExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during anonymous method expression visits.
        /// </summary>
        /// <param name="visitAnonymousMethodExpression">The function to call during anonymous method expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitAnonymousMethodExpression(Func<CSharpSyntaxRewriter, AnonymousMethodExpressionSyntax, SyntaxNode> visitAnonymousMethodExpression = default)
        {
            _visitAnonymousMethodExpression = visitAnonymousMethodExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during simple lambda expression visits.
        /// </summary>
        /// <param name="visitSimpleLambdaExpression">The function to call during simple lambda expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitSimpleLambdaExpression(Func<CSharpSyntaxRewriter, SimpleLambdaExpressionSyntax, SyntaxNode> visitSimpleLambdaExpression = default)
        {
            _visitSimpleLambdaExpression = visitSimpleLambdaExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during ref expression visits.
        /// </summary>
        /// <param name="visitRefExpression">The function to call during ref expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitRefExpression(Func<CSharpSyntaxRewriter, RefExpressionSyntax, SyntaxNode> visitRefExpression = default)
        {
            _visitRefExpression = visitRefExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during parenthesized lambda expression visits.
        /// </summary>
        /// <param name="visitParenthesizedLambdaExpression">The function to call during parenthesized lambda expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitParenthesizedLambdaExpression(Func<CSharpSyntaxRewriter, ParenthesizedLambdaExpressionSyntax, SyntaxNode> visitParenthesizedLambdaExpression = default)
        {
            _visitParenthesizedLambdaExpression = visitParenthesizedLambdaExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during initializer expression visits.
        /// </summary>
        /// <param name="visitInitializerExpression">The function to call during initializer expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitInitializerExpression(Func<CSharpSyntaxRewriter, InitializerExpressionSyntax, SyntaxNode> visitInitializerExpression = default)
        {
            _visitInitializerExpression = visitInitializerExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during implicit object creation expression visits.
        /// </summary>
        /// <param name="visitImplicitObjectCreationExpression">The function to call during implicit object creation expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitImplicitObjectCreationExpression(Func<CSharpSyntaxRewriter, ImplicitObjectCreationExpressionSyntax, SyntaxNode> visitImplicitObjectCreationExpression = default)
        {
            _visitImplicitObjectCreationExpression = visitImplicitObjectCreationExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during object creation expression visits.
        /// </summary>
        /// <param name="visitObjectCreationExpression">The function to call during object creation expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitObjectCreationExpression(Func<CSharpSyntaxRewriter, ObjectCreationExpressionSyntax, SyntaxNode> visitObjectCreationExpression = default)
        {
            _visitObjectCreationExpression = visitObjectCreationExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during with expression visits.
        /// </summary>
        /// <param name="visitWithExpression">The function to call during with expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitWithExpression(Func<CSharpSyntaxRewriter, WithExpressionSyntax, SyntaxNode> visitWithExpression = default)
        {
            _visitWithExpression = visitWithExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during anonymous object member declarator visits.
        /// </summary>
        /// <param name="visitAnonymousObjectMemberDeclarator">The function to call during anonymous object member declarator visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitAnonymousObjectMemberDeclarator(Func<CSharpSyntaxRewriter, AnonymousObjectMemberDeclaratorSyntax, SyntaxNode> visitAnonymousObjectMemberDeclarator = default)
        {
            _visitAnonymousObjectMemberDeclarator = visitAnonymousObjectMemberDeclarator;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during anonymous object creation expression visits.
        /// </summary>
        /// <param name="visitAnonymousObjectCreationExpression">The function to call during anonymous object creation expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitAnonymousObjectCreationExpression(Func<CSharpSyntaxRewriter, AnonymousObjectCreationExpressionSyntax, SyntaxNode> visitAnonymousObjectCreationExpression = default)
        {
            _visitAnonymousObjectCreationExpression = visitAnonymousObjectCreationExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during array creation expression visits.
        /// </summary>
        /// <param name="visitArrayCreationExpression">The function to call during array creation expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitArrayCreationExpression(Func<CSharpSyntaxRewriter, ArrayCreationExpressionSyntax, SyntaxNode> visitArrayCreationExpression = default)
        {
            _visitArrayCreationExpression = visitArrayCreationExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during implicit array creation expression visits.
        /// </summary>
        /// <param name="visitImplicitArrayCreationExpression">The function to call during implicit array creation expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter

         WithVisitImplicitArrayCreationExpression(Func<CSharpSyntaxRewriter, ImplicitArrayCreationExpressionSyntax, SyntaxNode> visitImplicitArrayCreationExpression = default)
        {
            _visitImplicitArrayCreationExpression = visitImplicitArrayCreationExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during stack alloc array creation expression visits.
        /// </summary>
        /// <param name="visitStackAllocArrayCreationExpression">The function to call during stack alloc array creation expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitStackAllocArrayCreationExpression(Func<CSharpSyntaxRewriter, StackAllocArrayCreationExpressionSyntax, SyntaxNode> visitStackAllocArrayCreationExpression = default)
        {
            _visitStackAllocArrayCreationExpression = visitStackAllocArrayCreationExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during implicit stack alloc array creation expression visits.
        /// </summary>
        /// <param name="visitImplicitStackAllocArrayCreationExpression">The function to call during implicit stack alloc array creation expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitImplicitStackAllocArrayCreationExpression(Func<CSharpSyntaxRewriter, ImplicitStackAllocArrayCreationExpressionSyntax, SyntaxNode> visitImplicitStackAllocArrayCreationExpression = default)
        {
            _visitImplicitStackAllocArrayCreationExpression = visitImplicitStackAllocArrayCreationExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during collection expression visits.
        /// </summary>
        /// <param name="visitCollectionExpression">The function to call during collection expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitCollectionExpression(Func<CSharpSyntaxRewriter, CollectionExpressionSyntax, SyntaxNode> visitCollectionExpression = default)
        {
            _visitCollectionExpression = visitCollectionExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during expression element visits.
        /// </summary>
        /// <param name="visitExpressionElement">The function to call during expression element visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitExpressionElement(Func<CSharpSyntaxRewriter, ExpressionElementSyntax, SyntaxNode> visitExpressionElement = default)
        {
            _visitExpressionElement = visitExpressionElement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during spread element visits.
        /// </summary>
        /// <param name="visitSpreadElement">The function to call during spread element visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitSpreadElement(Func<CSharpSyntaxRewriter, SpreadElementSyntax, SyntaxNode> visitSpreadElement = default)
        {
            _visitSpreadElement = visitSpreadElement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during query expression visits.
        /// </summary>
        /// <param name="visitQueryExpression">The function to call during query expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitQueryExpression(Func<CSharpSyntaxRewriter, QueryExpressionSyntax, SyntaxNode> visitQueryExpression = default)
        {
            _visitQueryExpression = visitQueryExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during query body visits.
        /// </summary>
        /// <param name="visitQueryBody">The function to call during query body visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitQueryBody(Func<CSharpSyntaxRewriter, QueryBodySyntax, SyntaxNode> visitQueryBody = default)
        {
            _visitQueryBody = visitQueryBody;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during from clause visits.
        /// </summary>
        /// <param name="visitFromClause">The function to call during from clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitFromClause(Func<CSharpSyntaxRewriter, FromClauseSyntax, SyntaxNode> visitFromClause = default)
        {
            _visitFromClause = visitFromClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during let clause visits.
        /// </summary>
        /// <param name="visitLetClause">The function to call during let clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitLetClause(Func<CSharpSyntaxRewriter, LetClauseSyntax, SyntaxNode> visitLetClause = default)
        {
            _visitLetClause = visitLetClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during join clause visits.
        /// </summary>
        /// <param name="visitJoinClause">The function to call during join clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitJoinClause(Func<CSharpSyntaxRewriter, JoinClauseSyntax, SyntaxNode> visitJoinClause = default)
        {
            _visitJoinClause = visitJoinClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during join into clause visits.
        /// </summary>
        /// <param name="visitJoinIntoClause">The function to call during join into clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitJoinIntoClause(Func<CSharpSyntaxRewriter, JoinIntoClauseSyntax, SyntaxNode> visitJoinIntoClause = default)
        {
            _visitJoinIntoClause = visitJoinIntoClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during where clause visits.
        /// </summary>
        /// <param name="visitWhereClause">The function to call during where clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitWhereClause(Func<CSharpSyntaxRewriter, WhereClauseSyntax, SyntaxNode> visitWhereClause = default)
        {
            _visitWhereClause = visitWhereClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during order by clause visits.
        /// </summary>
        /// <param name="visitOrderByClause">The function to call during order by clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitOrderByClause(Func<CSharpSyntaxRewriter, OrderByClauseSyntax, SyntaxNode> visitOrderByClause = default)
        {
            _visitOrderByClause = visitOrderByClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during ordering visits.
        /// </summary>
        /// <param name="visitOrdering">The function to call during ordering visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitOrdering(Func<CSharpSyntaxRewriter, OrderingSyntax, SyntaxNode> visitOrdering = default)
        {
            _visitOrdering = visitOrdering;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during select clause visits.
        /// </summary>
        /// <param name="visitSelectClause">The function to call during select clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitSelectClause(Func<CSharpSyntaxRewriter, SelectClauseSyntax, SyntaxNode> visitSelectClause = default)
        {
            _visitSelectClause = visitSelectClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during group clause visits.
        /// </summary>
        /// <param name="visitGroupClause">The function to call during group clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitGroupClause(Func<CSharpSyntaxRewriter, GroupClauseSyntax, SyntaxNode> visitGroupClause = default)
        {
            _visitGroupClause = visitGroupClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during query continuation visits.
        /// </summary>
        /// <param name="visitQueryContinuation">The function to call during query continuation visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitQueryContinuation(Func<CSharpSyntaxRewriter, QueryContinuationSyntax, SyntaxNode> visitQueryContinuation = default)
        {
            _visitQueryContinuation = visitQueryContinuation;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during omitted array size expression visits.
        /// </summary>
        /// <param name="visitOmittedArraySizeExpression">The function to call during omitted array size expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitOmittedArraySizeExpression(Func<CSharpSyntaxRewriter, OmittedArraySizeExpressionSyntax, SyntaxNode> visitOmittedArraySizeExpression = default)
        {
            _visitOmittedArraySizeExpression = visitOmittedArraySizeExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during interpolated string expression visits.
        /// </summary>
        /// <param name="visitInterpolatedStringExpression">The function to call during interpolated string expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitInterpolatedStringExpression(Func<CSharpSyntaxRewriter, InterpolatedStringExpressionSyntax, SyntaxNode> visitInterpolatedStringExpression = default)
        {
            _visitInterpolatedStringExpression = visitInterpolatedStringExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during is pattern expression visits.
        /// </summary>
        /// <param name="visitIsPatternExpression">The function to call during is pattern expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitIsPatternExpression(Func<CSharpSyntaxRewriter, IsPatternExpressionSyntax, SyntaxNode> visitIsPatternExpression = default)
        {
            _visitIsPatternExpression = visitIsPatternExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during throw expression visits.
        /// </summary>
        /// <param name="visitThrowExpression">The function to call during throw expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitThrowExpression(Func<CSharpSyntaxRewriter, ThrowExpressionSyntax, SyntaxNode> visitThrowExpression = default)
        {
            _visitThrowExpression = visitThrowExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during when clause visits.
        /// </summary>
        /// <param name="visitWhenClause">The function to call during when clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitWhenClause(Func<CSharpSyntaxRewriter, WhenClauseSyntax, SyntaxNode> visitWhenClause = default)
        {
            _visitWhenClause = visitWhenClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during discard pattern visits.
        /// </summary>
        /// <param name="visitDiscardPattern">The function to call during discard pattern visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitDiscardPattern(Func<CSharpSyntaxRewriter, DiscardPatternSyntax, SyntaxNode> visitDiscardPattern = default)
        {
            _visitDiscardPattern = visitDiscardPattern;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during declaration pattern visits.
        /// </summary>
        /// <param name="visitDeclarationPattern">The function to call during declaration pattern visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitDeclarationPattern(Func<CSharpSyntaxRewriter, DeclarationPatternSyntax, SyntaxNode> visitDeclarationPattern = default)
        {
            _visitDeclarationPattern = visitDeclarationPattern;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during var pattern visits.
        /// </summary>
        /// <param name="visitVarPattern">The function to call during var pattern visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitVarPattern(Func<CSharpSyntaxRewriter, VarPatternSyntax, SyntaxNode> visitVarPattern = default)
        {
            _visitVarPattern = visitVarPattern;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during recursive pattern visits.
        /// </summary>
        /// <param name="visitRecursivePattern">The function to call during recursive pattern visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitRecursivePattern(Func<CSharpSyntaxRewriter, RecursivePatternSyntax, SyntaxNode> visitRecursivePattern = default)
        {
            _visitRecursivePattern = visitRecursivePattern;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during positional pattern clause visits.
        /// </summary>
        /// <param name="visitPositionalPatternClause">The function to call during positional pattern clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitPositionalPatternClause(Func<CSharpSyntaxRewriter, PositionalPatternClauseSyntax, SyntaxNode> visitPositionalPatternClause = default)
        {
            _visitPositionalPatternClause = visitPositionalPatternClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during property pattern clause visits.
        /// </summary>
        /// <param name="visitPropertyPatternClause">The function to call during property pattern clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitPropertyPatternClause(Func<CSharpSyntaxRewriter, PropertyPatternClauseSyntax, SyntaxNode> visitPropertyPatternClause = default)
        {
            _visitPropertyPatternClause = visitPropertyPatternClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during subpattern visits.
        /// </summary>
        /// <param name="visitSubpattern">The function to call during subpattern visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitSubpattern(Func<CSharpSyntaxRewriter, SubpatternSyntax, SyntaxNode> visitSubpattern = default)
        {
            _visitSubpattern = visitSubpattern;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during constant pattern visits.
        /// </summary>
        /// <param name="visitConstantPattern">The function to call during constant pattern visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitConstantPattern(Func<CSharpSyntaxRewriter, ConstantPatternSyntax, SyntaxNode> visitConstantPattern = default)
        {
            _visitConstantPattern = visitConstantPattern;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during parenthesized pattern visits.
        /// </summary>
        /// <param name="visitParenthesizedPattern">The function to call during parenthesized pattern visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitParenthesizedPattern(Func<CSharpSyntaxRewriter, ParenthesizedPatternSyntax, SyntaxNode> visitParenthesizedPattern = default)
        {
            _visitParenthesizedPattern = visitParenthesizedPattern;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during relational pattern visits.
        /// </summary>
        /// <param name="visitRelationalPattern">The function to call during relational pattern visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitRelationalPattern(Func<CSharpSyntaxRewriter, RelationalPatternSyntax, SyntaxNode> visitRelationalPattern = default)
        {
            _visitRelationalPattern = visitRelationalPattern;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during type pattern visits.
        /// </summary>
        /// <param name="visitTypePattern">The function to call during type pattern visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitTypePattern(Func<CSharpSyntaxRewriter, TypePatternSyntax, SyntaxNode> visitTypePattern = default)
        {
            _visitTypePattern = visitTypePattern;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during binary pattern visits.
        /// </summary>
        /// <param name="visitBinaryPattern">The function to call during binary pattern visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitBinaryPattern(Func<CSharpSyntaxRewriter, BinaryPatternSyntax, SyntaxNode> visitBinaryPattern = default)
        {
            _visitBinaryPattern = visitBinaryPattern;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during unary pattern visits.
        /// </summary>
        /// <param name="visitUnaryPattern">The function to call during unary pattern visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitUnaryPattern(Func<CSharpSyntaxRewriter, UnaryPatternSyntax, SyntaxNode> visitUnaryPattern = default)
        {
            _visitUnaryPattern = visitUnaryPattern;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during list pattern visits.
        /// </summary>
        /// <param name="visitListPattern">The function to call during list pattern visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitListPattern(Func<CSharpSyntaxRewriter, ListPatternSyntax, SyntaxNode> visitListPattern = default)
        {
            _visitListPattern = visitListPattern;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during slice pattern visits.
        /// </summary>
        /// <param name="visitSlicePattern">The function to call during slice pattern visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitSlicePattern(Func<CSharpSyntaxRewriter, SlicePatternSyntax, SyntaxNode> visitSlicePattern = default)
        {
            _visitSlicePattern = visitSlicePattern;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during interpolated string text visits.
        /// </summary>
        /// <param name="visitInterpolatedStringText">The function to call during interpolated string text visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitInterpolatedStringText(Func<CSharpSyntaxRewriter, InterpolatedStringTextSyntax, SyntaxNode> visitInterpolatedStringText = default)
        {
            _visitInterpolatedStringText = visitInterpolatedStringText;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during interpolation visits.
        /// </summary>
        /// <param name="visitInterpolation">The function to call during interpolation visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitInterpolation(Func<CSharpSyntaxRewriter, InterpolationSyntax, SyntaxNode> visitInterpolation = default)
        {
            _visitInterpolation = visitInterpolation;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during interpolation alignment clause visits.
        /// </summary>
        /// <param name="visitInterpolationAlignmentClause">The function to call during interpolation alignment clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitInterpolationAlignmentClause(Func<CSharpSyntaxRewriter, InterpolationAlignmentClauseSyntax, SyntaxNode> visitInterpolationAlignmentClause = default)
        {
            _visitInterpolationAlignmentClause = visitInterpolationAlignmentClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during interpolation format clause visits.
        /// </summary>
        /// <param name="visitInterpolationFormatClause">The function to call during interpolation format clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitInterpolationFormatClause(Func<CSharpSyntaxRewriter, InterpolationFormatClauseSyntax, SyntaxNode> visitInterpolationFormatClause = default)
        {
            _visitInterpolationFormatClause = visitInterpolationFormatClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during global statement visits.
        /// </summary>
        /// <param name="visitGlobalStatement">The function to call during global statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitGlobalStatement(Func<CSharpSyntaxRewriter, GlobalStatementSyntax, SyntaxNode> visitGlobalStatement = default)
        {
            _visitGlobalStatement = visitGlobalStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during block visits.
        /// </summary>
        /// <param name="visitBlock">The function to call during block visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitBlock(Func<CSharpSyntaxRewriter, BlockSyntax, SyntaxNode> visitBlock = default)
        {
            _visitBlock = visitBlock;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during local function statement visits.
        /// </summary>
        /// <param name="visitLocalFunctionStatement">The function to call during local function statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitLocalFunctionStatement(Func<CSharpSyntaxRewriter, LocalFunctionStatementSyntax, SyntaxNode> visitLocalFunctionStatement = default)
        {
            _visitLocalFunctionStatement = visitLocalFunctionStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during local declaration statement visits.
        /// </summary>
        /// <param name="visitLocalDeclarationStatement">The function to call during local declaration statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitLocalDeclarationStatement(Func<CSharpSyntaxRewriter, LocalDeclarationStatementSyntax, SyntaxNode> visitLocalDeclarationStatement = default)
        {
            _visitLocalDeclarationStatement = visitLocalDeclarationStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during variable declaration visits.
        /// </summary>
        /// <param name="visitVariableDeclaration">The function to call during variable declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitVariableDeclaration(Func<CSharpSyntaxRewriter, VariableDeclarationSyntax, SyntaxNode> visitVariableDeclaration = default)
        {
            _visitVariableDeclaration = visitVariableDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during variable declarator visits.
        /// </summary>
        /// <param name="visitVariableDeclarator">The function to call during variable declarator visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitVariableDeclarator(Func<CSharpSyntaxRewriter, VariableDeclaratorSyntax, SyntaxNode> visitVariableDeclarator = default)
        {
            _visitVariableDeclarator = visitVariableDeclarator;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during equals value clause visits.
        /// </summary>
        /// <param name="visitEqualsValueClause">The function to call during equals value clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitEqualsValueClause(Func<CSharpSyntaxRewriter, EqualsValueClauseSyntax, SyntaxNode> visitEqualsValueClause = default)
        {
            _visitEqualsValueClause = visitEqualsValueClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during single variable designation visits.
        /// </summary>
        /// <param name="visitSingleVariableDesignation">The function to call during single variable designation visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitSingleVariableDesignation(Func<CSharpSyntaxRewriter, SingleVariableDesignationSyntax, SyntaxNode> visitSingleVariableDesignation = default)
        {
            _visitSingleVariableDesignation = visitSingleVariableDesignation;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during discard designation visits.
        /// </summary>
        /// <param name="visitDiscardDesignation">The function to call during discard designation visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitDiscardDesignation(Func<CSharpSyntaxRewriter, DiscardDesignationSyntax, SyntaxNode> visitDiscardDesignation = default)
        {
            _visitDiscardDesignation = visitDiscardDesignation;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during parenthesized variable designation visits.
        /// </summary>
        /// <param name="visitParenthesizedVariableDesignation">The function to call during parenthesized variable designation visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitParenthesizedVariableDesignation(Func<CSharpSyntaxRewriter, ParenthesizedVariableDesignationSyntax, SyntaxNode> visitParenthesizedVariableDesignation = default)
        {
            _visitParenthesizedVariableDesignation = visitParenthesizedVariableDesignation;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during expression statement visits.
        /// </summary>
        /// <param name="visitExpressionStatement">The function to call during expression statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitExpressionStatement(Func<CSharpSyntaxRewriter, ExpressionStatementSyntax, SyntaxNode> visitExpressionStatement = default)
        {
            _visitExpressionStatement = visitExpressionStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during empty statement visits.
        /// </summary>
        /// <param name="visitEmptyStatement">The function to call during empty statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitEmptyStatement(Func<CSharpSyntaxRewriter, EmptyStatementSyntax, SyntaxNode> visitEmptyStatement = default)
        {
            _visitEmptyStatement = visitEmptyStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during labeled statement visits.
        /// </summary>
        /// <param name="visitLabeledStatement">The function to call during labeled statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitLabeledStatement(Func<CSharpSyntaxRewriter, LabeledStatementSyntax, SyntaxNode> visitLabeledStatement = default)
        {
            _visitLabeledStatement = visitLabeledStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during goto statement visits.
        /// </summary>
        /// <param name="visitGotoStatement">The function to call during goto statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitGotoStatement(Func<CSharpSyntaxRewriter, GotoStatementSyntax, SyntaxNode> visitGotoStatement = default)
        {
            _visitGotoStatement = visitGotoStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during break statement visits.
        /// </summary>
        /// <param name="visitBreakStatement">The function to call during break statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitBreakStatement(Func<CSharpSyntaxRewriter, BreakStatementSyntax, SyntaxNode> visitBreakStatement = default)
        {
            _visitBreakStatement = visitBreakStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during continue statement visits.
        /// </summary>
        /// <param name="visitContinueStatement">The function to call during continue statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitContinueStatement(Func<CSharpSyntaxRewriter, ContinueStatementSyntax, SyntaxNode> visitContinueStatement = default)
        {
            _visitContinueStatement = visitContinueStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during return statement visits.
        /// </summary>
        /// <param name="visitReturnStatement">The function to call during return statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitReturnStatement(Func<CSharpSyntaxRewriter, ReturnStatementSyntax, SyntaxNode> visitReturnStatement = default)
        {
            _visitReturnStatement = visitReturnStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during throw statement visits.
        /// </summary>
        /// <param name="visitThrowStatement">The function to call during throw statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitThrowStatement(Func<CSharpSyntaxRewriter, ThrowStatementSyntax, SyntaxNode> visitThrowStatement = default)
        {
            _visitThrowStatement = visitThrowStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during yield statement visits.
        /// </summary>
        /// <param name="visitYieldStatement">The function to call during yield statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitYieldStatement(Func<CSharpSyntaxRewriter, YieldStatementSyntax, SyntaxNode> visitYieldStatement = default)
        {
            _visitYieldStatement = visitYieldStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during while statement visits.
        /// </summary>
        /// <param name="visitWhileStatement">The function to call during while statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitWhileStatement(Func<CSharpSyntaxRewriter, WhileStatementSyntax, SyntaxNode> visitWhileStatement = default)
        {
            _visitWhileStatement = visitWhileStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during do statement visits.
        /// </summary>
        /// <param name="visitDoStatement">The function to call during do statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitDoStatement(Func<CSharpSyntaxRewriter, DoStatementSyntax, SyntaxNode> visitDoStatement = default)
        {
            _visitDoStatement = visitDoStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during for statement visits.
        /// </summary>
        /// <param name="visitForStatement">The function to call during for statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitForStatement(Func<CSharpSyntaxRewriter, ForStatementSyntax, SyntaxNode> visitForStatement = default)
        {
            _visitForStatement = visitForStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during foreach statement visits.
        /// </summary>
        /// <param name="visitForEachStatement">The function to call during foreach statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitForEachStatement(Func<CSharpSyntaxRewriter, ForEachStatementSyntax, SyntaxNode> visitForEachStatement = default)
        {
            _visitForEachStatement = visitForEachStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during foreach variable statement visits.
        /// </summary>
        /// <param name="visitForEachVariableStatement">The function to call during foreach variable statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitForEachVariableStatement(Func<CSharpSyntaxRewriter, ForEachVariableStatementSyntax, SyntaxNode> visitForEachVariableStatement = default)
        {
            _visitForEachVariableStatement = visitForEachVariableStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during using statement visits.
        /// </summary>
        /// <param name="visitUsingStatement">The function to call during using statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitUsingStatement(Func<CSharpSyntaxRewriter, UsingStatementSyntax, SyntaxNode> visitUsingStatement = default)
        {
            _visitUsingStatement = visitUsingStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during fixed statement visits.
        /// </summary>
        /// <param name="visitFixedStatement">The function to call during fixed statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitFixedStatement(Func<CSharpSyntaxRewriter, FixedStatementSyntax, SyntaxNode> visitFixedStatement = default)
        {
            _visitFixedStatement = visitFixedStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during checked statement visits.
        /// </summary>
        /// <param name="visitCheckedStatement">The function to call during checked statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitCheckedStatement(Func<CSharpSyntaxRewriter, CheckedStatementSyntax, SyntaxNode> visitCheckedStatement = default)
        {
            _visitCheckedStatement = visitCheckedStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during unsafe statement visits.
        /// </summary>
        /// <param name="visitUnsafeStatement">The function to call during unsafe statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitUnsafeStatement(Func<CSharpSyntaxRewriter, UnsafeStatementSyntax, SyntaxNode> visitUnsafeStatement = default)
        {
            _visitUnsafeStatement = visitUnsafeStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during lock statement visits.
        /// </summary>
        /// <param name="visitLockStatement">The function to call during lock statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitLockStatement(Func<CSharpSyntaxRewriter, LockStatementSyntax, SyntaxNode> visitLockStatement = default)
        {
            _visitLockStatement = visitLockStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during if statement visits.
        /// </summary>
        /// <param name="visitIfStatement">The function to call during if statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitIfStatement(Func<CSharpSyntaxRewriter, IfStatementSyntax, SyntaxNode> visitIfStatement = default)
        {
            _visitIfStatement = visitIfStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during else clause visits.
        /// </summary>
        /// <param name="visitElseClause">The function to call during else clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitElseClause(Func<CSharpSyntaxRewriter, ElseClauseSyntax, SyntaxNode> visitElseClause = default)
        {
            _visitElseClause = visitElseClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during switch statement visits.
        /// </summary>
        /// <param name="visitSwitchStatement">The function to call during switch statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitSwitchStatement(Func<CSharpSyntaxRewriter, SwitchStatementSyntax, SyntaxNode> visitSwitchStatement = default)
        {
            _visitSwitchStatement = visitSwitchStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during switch section visits.
        /// </summary>
        /// <param name="visitSwitchSection">The function to call during switch section visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitSwitchSection(Func<CSharpSyntaxRewriter, SwitchSectionSyntax, SyntaxNode> visitSwitchSection = default)
        {
            _visitSwitchSection = visitSwitchSection;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during case pattern switch label visits.
        /// </summary>
        /// <param name="visitCasePatternSwitchLabel">The function to call during case pattern switch label visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitCasePatternSwitchLabel(Func<CSharpSyntaxRewriter, CasePatternSwitchLabelSyntax, SyntaxNode> visitCasePatternSwitchLabel = default)
        {
            _visitCasePatternSwitchLabel = visitCasePatternSwitchLabel;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during case switch label visits.
        /// </summary>
        /// <param name="visitCaseSwitchLabel">The function to call during case switch label visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitCaseSwitchLabel(Func<CSharpSyntaxRewriter, CaseSwitchLabelSyntax, SyntaxNode> visitCaseSwitchLabel = default)
        {
            _visitCaseSwitchLabel = visitCaseSwitchLabel;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during default switch label visits.
        /// </summary>
        /// <param name="visitDefaultSwitchLabel">The function to call during default switch label visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitDefaultSwitchLabel(Func<CSharpSyntaxRewriter, DefaultSwitchLabelSyntax, SyntaxNode> visitDefaultSwitchLabel = default)
        {
            _visitDefaultSwitchLabel = visitDefaultSwitchLabel;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during switch expression visits.
        /// </summary>
        /// <param name="visitSwitchExpression">The function to call during switch expression visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitSwitchExpression(Func<CSharpSyntaxRewriter, SwitchExpressionSyntax, SyntaxNode> visitSwitchExpression = default)
        {
            _visitSwitchExpression = visitSwitchExpression;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during switch expression arm visits.
        /// </summary>
        /// <param name="visitSwitchExpressionArm">The function to call during switch expression arm visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitSwitchExpressionArm(Func<CSharpSyntaxRewriter, SwitchExpressionArmSyntax, SyntaxNode> visitSwitchExpressionArm = default)
        {
            _visitSwitchExpressionArm = visitSwitchExpressionArm;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during try statement visits.
        /// </summary>
        /// <param name="visitTryStatement">The function to call during try statement visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitTryStatement(Func<CSharpSyntaxRewriter, TryStatementSyntax, SyntaxNode> visitTryStatement = default)
        {
            _visitTryStatement = visitTryStatement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during catch clause visits.
        /// </summary>
        /// <param name="visitCatchClause">The function to call during catch clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitCatchClause(Func<CSharpSyntaxRewriter, CatchClauseSyntax, SyntaxNode> visitCatchClause = default)
        {
            _visitCatchClause = visitCatchClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during catch declaration visits.
        /// </summary>
        /// <param name="visitCatchDeclaration">The function to call during catch declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitCatchDeclaration(Func<CSharpSyntaxRewriter, CatchDeclarationSyntax, SyntaxNode> visitCatchDeclaration = default)
        {
            _visitCatchDeclaration = visitCatchDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during catch filter clause visits.
        /// </summary>
        /// <param name="visitCatchFilterClause">The function to call during catch filter clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitCatchFilterClause(Func<CSharpSyntaxRewriter, CatchFilterClauseSyntax, SyntaxNode> visitCatchFilterClause = default)
        {
            _visitCatchFilterClause = visitCatchFilterClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during finally clause visits.
        /// </summary>
        /// <param name="visitFinallyClause">The function to call during finally clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitFinallyClause(Func<CSharpSyntaxRewriter, FinallyClauseSyntax, SyntaxNode> visitFinallyClause = default)
        {
            _visitFinallyClause = visitFinallyClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during compilation unit visits.
        /// </summary>
        /// <param name="visitCompilationUnit">The function to call during compilation unit visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitCompilationUnit(Func<CSharpSyntaxRewriter, CompilationUnitSyntax, SyntaxNode> visitCompilationUnit = default)
        {
            _visitCompilationUnit = visitCompilationUnit;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during extern alias directive visits.
        /// </summary>
        /// <param name="visitExternAliasDirective">The function to call during extern alias directive visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitExternAliasDirective(Func<CSharpSyntaxRewriter, ExternAliasDirectiveSyntax, SyntaxNode> visitExternAliasDirective = default)
        {
            _visitExternAliasDirective = visitExternAliasDirective;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during using directive visits.
        /// </summary>
        /// <param name="visitUsingDirective">The function to call during using directive visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitUsingDirective(Func<CSharpSyntaxRewriter, UsingDirectiveSyntax, SyntaxNode> visitUsingDirective = default)
        {
            _visitUsingDirective = visitUsingDirective;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during namespace declaration visits.
        /// </summary>
        /// <param name="visitNamespaceDeclaration">The function to call during namespace declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitNamespaceDeclaration(Func<CSharpSyntaxRewriter, NamespaceDeclarationSyntax, SyntaxNode> visitNamespaceDeclaration = default)
        {
            _visitNamespaceDeclaration = visitNamespaceDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during file scoped namespace declaration visits.
        /// </summary>
        /// <param name="visitFileScopedNamespaceDeclaration">The function to call during file scoped namespace declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitFileScopedNamespaceDeclaration(Func<CSharpSyntaxRewriter, FileScopedNamespaceDeclarationSyntax, SyntaxNode> visitFileScopedNamespaceDeclaration = default)
        {
            _visitFileScopedNamespaceDeclaration = visitFileScopedNamespaceDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during attribute list visits.
        /// </summary>
        /// <param name="visitAttributeList">The function to call during attribute list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitAttributeList(Func<CSharpSyntaxRewriter, AttributeListSyntax, SyntaxNode> visitAttributeList = default)
        {
            _visitAttributeList = visitAttributeList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during attribute target specifier visits.
        /// </summary>
        /// <param name="visitAttributeTargetSpecifier">The function to call during attribute target specifier visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitAttributeTargetSpecifier(Func<CSharpSyntaxRewriter, AttributeTargetSpecifierSyntax, SyntaxNode> visitAttributeTargetSpecifier = default)
        {
            _visitAttributeTargetSpecifier = visitAttributeTargetSpecifier;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during attribute visits.
        /// </summary>
        /// <param name="visitAttribute">The function to call during attribute visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitAttribute(Func<CSharpSyntaxRewriter, AttributeSyntax, SyntaxNode> visitAttribute = default)
        {
            _visitAttribute = visitAttribute;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during attribute argument list visits.
        /// </summary>
        /// <param name="visitAttributeArgumentList">The function to call during attribute argument list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitAttributeArgumentList(Func<CSharpSyntaxRewriter, AttributeArgumentListSyntax, SyntaxNode> visitAttributeArgumentList = default)
        {
            _visitAttributeArgumentList = visitAttributeArgumentList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during attribute argument visits.
        /// </summary>
        /// <param name="visitAttributeArgument">The function to call during attribute argument visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitAttributeArgument(Func<CSharpSyntaxRewriter, AttributeArgumentSyntax, SyntaxNode> visitAttributeArgument = default)
        {
            _visitAttributeArgument = visitAttributeArgument;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during name equals visits.
        /// </summary>
        /// <param name="visitNameEquals">The function to call during name equals visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitNameEquals(Func<CSharpSyntaxRewriter, NameEqualsSyntax, SyntaxNode> visitNameEquals = default)
        {
            _visitNameEquals = visitNameEquals;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during type parameter list visits.
        /// </summary>
        /// <param name="visitTypeParameterList">The function to call during type parameter list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitTypeParameterList(Func<CSharpSyntaxRewriter, TypeParameterListSyntax, SyntaxNode> visitTypeParameterList = default)
        {
            _visitTypeParameterList = visitTypeParameterList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during type parameter visits.
        /// </summary>
        /// <param name="visitTypeParameter">The function to call during type parameter visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitTypeParameter(Func<CSharpSyntaxRewriter, TypeParameterSyntax, SyntaxNode> visitTypeParameter = default)
        {
            _visitTypeParameter = visitTypeParameter;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during class declaration visits.
        /// </summary>
        /// <param name="visitClassDeclaration">The function to call during class declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitClassDeclaration(Func<CSharpSyntaxRewriter, ClassDeclarationSyntax, SyntaxNode> visitClassDeclaration = default)
        {
            _visitClassDeclaration = visitClassDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during struct declaration visits.
        /// </summary>
        /// <param name="visitStructDeclaration">The function to call during struct declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitStructDeclaration(Func<CSharpSyntaxRewriter, StructDeclarationSyntax, SyntaxNode> visitStructDeclaration = default)
        {
            _visitStructDeclaration = visitStructDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during interface declaration visits.
        /// </summary>
        /// <param name="visitInterfaceDeclaration">The function to call during interface declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitInterfaceDeclaration(Func<CSharpSyntaxRewriter, InterfaceDeclarationSyntax, SyntaxNode> visitInterfaceDeclaration = default)
        {
            _visitInterfaceDeclaration = visitInterfaceDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during record declaration visits.
        /// </summary>
        /// <param name="visitRecordDeclaration">The function to call during record declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitRecordDeclaration(Func<CSharpSyntaxRewriter, RecordDeclarationSyntax, SyntaxNode> visitRecordDeclaration = default)
        {
            _visitRecordDeclaration = visitRecordDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during enum declaration visits.
        /// </summary>
        /// <param name="visitEnumDeclaration">The function to call during enum declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitEnumDeclaration(Func<CSharpSyntaxRewriter, EnumDeclarationSyntax, SyntaxNode> visitEnumDeclaration = default)
        {
            _visitEnumDeclaration = visitEnumDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during delegate declaration visits.
        /// </summary>
        /// <param name="visitDelegateDeclaration">The function to call during delegate declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitDelegateDeclaration(Func<CSharpSyntaxRewriter, DelegateDeclarationSyntax, SyntaxNode> visitDelegateDeclaration = default)
        {
            _visitDelegateDeclaration = visitDelegateDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during enum member declaration visits.
        /// </summary>
        /// <param name="visitEnumMemberDeclaration">The function to call during enum member declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitEnumMemberDeclaration(Func<CSharpSyntaxRewriter, EnumMemberDeclarationSyntax, SyntaxNode> visitEnumMemberDeclaration = default)
        {
            _visitEnumMemberDeclaration = visitEnumMemberDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during base list visits.
        /// </summary>
        /// <param name="visitBaseList">The function to call during base list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitBaseList(Func<CSharpSyntaxRewriter, BaseListSyntax, SyntaxNode> visitBaseList = default)
        {
            _visitBaseList = visitBaseList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during simple base type visits.
        /// </summary>
        /// <param name="visitSimpleBaseType">The function to call during simple base type visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitSimpleBaseType(Func<CSharpSyntaxRewriter, SimpleBaseTypeSyntax, SyntaxNode> visitSimpleBaseType = default)
        {
            _visitSimpleBaseType = visitSimpleBaseType;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during primary constructor base type visits.
        /// </summary>
        /// <param name="visitPrimaryConstructorBaseType">The function to call during primary constructor base type visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitPrimaryConstructorBaseType(Func<CSharpSyntaxRewriter, PrimaryConstructorBaseTypeSyntax, SyntaxNode> visitPrimaryConstructorBaseType = default)
        {
            _visitPrimaryConstructorBaseType = visitPrimaryConstructorBaseType;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during type parameter constraint clause visits.
        /// </summary>
        /// <param name="visitTypeParameterConstraintClause">The function to call during type parameter constraint clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitTypeParameterConstraintClause(Func<CSharpSyntaxRewriter, TypeParameterConstraintClauseSyntax, SyntaxNode> visitTypeParameterConstraintClause = default)
        {
            _visitTypeParameterConstraintClause = visitTypeParameterConstraintClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during constructor constraint visits.
        /// </summary>
        /// <param name="visitConstructorConstraint">The function to call during constructor constraint visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitConstructorConstraint(Func<CSharpSyntaxRewriter, ConstructorConstraintSyntax, SyntaxNode> visitConstructorConstraint = default)
        {
            _visitConstructorConstraint = visitConstructorConstraint;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during class or struct constraint visits.
        /// </summary>
        /// <param name="visitClassOrStructConstraint">The function to call during class or struct constraint visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitClassOrStructConstraint(Func<CSharpSyntaxRewriter, ClassOrStructConstraintSyntax, SyntaxNode> visitClassOrStructConstraint = default)
        {
            _visitClassOrStructConstraint = visitClassOrStructConstraint;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during type constraint visits.
        /// </summary>
        /// <param name="visitTypeConstraint">The function to call during type constraint visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitTypeConstraint(Func<CSharpSyntaxRewriter, TypeConstraintSyntax, SyntaxNode> visitTypeConstraint = default)
        {
            _visitTypeConstraint = visitTypeConstraint;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during default constraint visits.
        /// </summary>
        /// <param name="visitDefaultConstraint">The function to call during default constraint visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitDefaultConstraint(Func<CSharpSyntaxRewriter, DefaultConstraintSyntax, SyntaxNode> visitDefaultConstraint = default)
        {
            _visitDefaultConstraint = visitDefaultConstraint;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during field declaration visits.
        /// </summary>
        /// <param name="visitFieldDeclaration">The function to call during field declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitFieldDeclaration(Func<CSharpSyntaxRewriter, FieldDeclarationSyntax, SyntaxNode> visitFieldDeclaration = default)
        {
            _visitFieldDeclaration = visitFieldDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during event field declaration visits.
        /// </summary>
        /// <param name="visitEventFieldDeclaration">The function to call during event field declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual

         FluentCSharpSyntaxRewriter WithVisitEventFieldDeclaration(Func<CSharpSyntaxRewriter, EventFieldDeclarationSyntax, SyntaxNode> visitEventFieldDeclaration = default)
        {
            _visitEventFieldDeclaration = visitEventFieldDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during explicit interface specifier visits.
        /// </summary>
        /// <param name="visitExplicitInterfaceSpecifier">The function to call during explicit interface specifier visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitExplicitInterfaceSpecifier(Func<CSharpSyntaxRewriter, ExplicitInterfaceSpecifierSyntax, SyntaxNode> visitExplicitInterfaceSpecifier = default)
        {
            _visitExplicitInterfaceSpecifier = visitExplicitInterfaceSpecifier;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during method declaration visits.
        /// </summary>
        /// <param name="visitMethodDeclaration">The function to call during method declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitMethodDeclaration(Func<CSharpSyntaxRewriter, MethodDeclarationSyntax, SyntaxNode> visitMethodDeclaration = default)
        {
            _visitMethodDeclaration = visitMethodDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during operator declaration visits.
        /// </summary>
        /// <param name="visitOperatorDeclaration">The function to call during operator declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitOperatorDeclaration(Func<CSharpSyntaxRewriter, OperatorDeclarationSyntax, SyntaxNode> visitOperatorDeclaration = default)
        {
            _visitOperatorDeclaration = visitOperatorDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during conversion operator declaration visits.
        /// </summary>
        /// <param name="visitConversionOperatorDeclaration">The function to call during conversion operator declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitConversionOperatorDeclaration(Func<CSharpSyntaxRewriter, ConversionOperatorDeclarationSyntax, SyntaxNode> visitConversionOperatorDeclaration = default)
        {
            _visitConversionOperatorDeclaration = visitConversionOperatorDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during constructor declaration visits.
        /// </summary>
        /// <param name="visitConstructorDeclaration">The function to call during constructor declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitConstructorDeclaration(Func<CSharpSyntaxRewriter, ConstructorDeclarationSyntax, SyntaxNode> visitConstructorDeclaration = default)
        {
            _visitConstructorDeclaration = visitConstructorDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during constructor initializer visits.
        /// </summary>
        /// <param name="visitConstructorInitializer">The function to call during constructor initializer visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitConstructorInitializer(Func<CSharpSyntaxRewriter, ConstructorInitializerSyntax, SyntaxNode> visitConstructorInitializer = default)
        {
            _visitConstructorInitializer = visitConstructorInitializer;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during destructor declaration visits.
        /// </summary>
        /// <param name="visitDestructorDeclaration">The function to call during destructor declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitDestructorDeclaration(Func<CSharpSyntaxRewriter, DestructorDeclarationSyntax, SyntaxNode> visitDestructorDeclaration = default)
        {
            _visitDestructorDeclaration = visitDestructorDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during property declaration visits.
        /// </summary>
        /// <param name="visitPropertyDeclaration">The function to call during property declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitPropertyDeclaration(Func<CSharpSyntaxRewriter, PropertyDeclarationSyntax, SyntaxNode> visitPropertyDeclaration = default)
        {
            _visitPropertyDeclaration = visitPropertyDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during arrow expression clause visits.
        /// </summary>
        /// <param name="visitArrowExpressionClause">The function to call during arrow expression clause visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitArrowExpressionClause(Func<CSharpSyntaxRewriter, ArrowExpressionClauseSyntax, SyntaxNode> visitArrowExpressionClause = default)
        {
            _visitArrowExpressionClause = visitArrowExpressionClause;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during event declaration visits.
        /// </summary>
        /// <param name="visitEventDeclaration">The function to call during event declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitEventDeclaration(Func<CSharpSyntaxRewriter, EventDeclarationSyntax, SyntaxNode> visitEventDeclaration = default)
        {
            _visitEventDeclaration = visitEventDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during indexer declaration visits.
        /// </summary>
        /// <param name="visitIndexerDeclaration">The function to call during indexer declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitIndexerDeclaration(Func<CSharpSyntaxRewriter, IndexerDeclarationSyntax, SyntaxNode> visitIndexerDeclaration = default)
        {
            _visitIndexerDeclaration = visitIndexerDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during accessor list visits.
        /// </summary>
        /// <param name="visitAccessorList">The function to call during accessor list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitAccessorList(Func<CSharpSyntaxRewriter, AccessorListSyntax, SyntaxNode> visitAccessorList = default)
        {
            _visitAccessorList = visitAccessorList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during accessor declaration visits.
        /// </summary>
        /// <param name="visitAccessorDeclaration">The function to call during accessor declaration visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitAccessorDeclaration(Func<CSharpSyntaxRewriter, AccessorDeclarationSyntax, SyntaxNode> visitAccessorDeclaration = default)
        {
            _visitAccessorDeclaration = visitAccessorDeclaration;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during parameter list visits.
        /// </summary>
        /// <param name="visitParameterList">The function to call during parameter list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitParameterList(Func<CSharpSyntaxRewriter, ParameterListSyntax, SyntaxNode> visitParameterList = default)
        {
            _visitParameterList = visitParameterList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during bracketed parameter list visits.
        /// </summary>
        /// <param name="visitBracketedParameterList">The function to call during bracketed parameter list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitBracketedParameterList(Func<CSharpSyntaxRewriter, BracketedParameterListSyntax, SyntaxNode> visitBracketedParameterList = default)
        {
            _visitBracketedParameterList = visitBracketedParameterList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during parameter visits.
        /// </summary>
        /// <param name="visitParameter">The function to call during parameter visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitParameter(Func<CSharpSyntaxRewriter, ParameterSyntax, SyntaxNode> visitParameter = default)
        {
            _visitParameter = visitParameter;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during function pointer parameter visits.
        /// </summary>
        /// <param name="visitFunctionPointerParameter">The function to call during function pointer parameter visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitFunctionPointerParameter(Func<CSharpSyntaxRewriter, FunctionPointerParameterSyntax, SyntaxNode> visitFunctionPointerParameter = default)
        {
            _visitFunctionPointerParameter = visitFunctionPointerParameter;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during incomplete member visits.
        /// </summary>
        /// <param name="visitIncompleteMember">The function to call during incomplete member visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitIncompleteMember(Func<CSharpSyntaxRewriter, IncompleteMemberSyntax, SyntaxNode> visitIncompleteMember = default)
        {
            _visitIncompleteMember = visitIncompleteMember;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during skipped tokens trivia visits.
        /// </summary>
        /// <param name="visitSkippedTokensTrivia">The function to call during skipped tokens trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitSkippedTokensTrivia(Func<CSharpSyntaxRewriter, SkippedTokensTriviaSyntax, SyntaxNode> visitSkippedTokensTrivia = default)
        {
            _visitSkippedTokensTrivia = visitSkippedTokensTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during documentation comment trivia visits.
        /// </summary>
        /// <param name="visitDocumentationCommentTrivia">The function to call during documentation comment trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitDocumentationCommentTrivia(Func<CSharpSyntaxRewriter, DocumentationCommentTriviaSyntax, SyntaxNode> visitDocumentationCommentTrivia = default)
        {
            _visitDocumentationCommentTrivia = visitDocumentationCommentTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during type cref visits.
        /// </summary>
        /// <param name="visitTypeCref">The function to call during type cref visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitTypeCref(Func<CSharpSyntaxRewriter, TypeCrefSyntax, SyntaxNode> visitTypeCref = default)
        {
            _visitTypeCref = visitTypeCref;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during qualified cref visits.
        /// </summary>
        /// <param name="visitQualifiedCref">The function to call during qualified cref visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitQualifiedCref(Func<CSharpSyntaxRewriter, QualifiedCrefSyntax, SyntaxNode> visitQualifiedCref = default)
        {
            _visitQualifiedCref = visitQualifiedCref;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during name member cref visits.
        /// </summary>
        /// <param name="visitNameMemberCref">The function to call during name member cref visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitNameMemberCref(Func<CSharpSyntaxRewriter, NameMemberCrefSyntax, SyntaxNode> visitNameMemberCref = default)
        {
            _visitNameMemberCref = visitNameMemberCref;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during indexer member cref visits.
        /// </summary>
        /// <param name="visitIndexerMemberCref">The function to call during indexer member cref visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitIndexerMemberCref(Func<CSharpSyntaxRewriter, IndexerMemberCrefSyntax, SyntaxNode> visitIndexerMemberCref = default)
        {
            _visitIndexerMemberCref = visitIndexerMemberCref;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during operator member cref visits.
        /// </summary>
        /// <param name="visitOperatorMemberCref">The function to call during operator member cref visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitOperatorMemberCref(Func<CSharpSyntaxRewriter, OperatorMemberCrefSyntax, SyntaxNode> visitOperatorMemberCref = default)
        {
            _visitOperatorMemberCref = visitOperatorMemberCref;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during conversion operator member cref visits.
        /// </summary>
        /// <param name="visitConversionOperatorMemberCref">The function to call during conversion operator member cref visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitConversionOperatorMemberCref(Func<CSharpSyntaxRewriter, ConversionOperatorMemberCrefSyntax, SyntaxNode> visitConversionOperatorMemberCref = default)
        {
            _visitConversionOperatorMemberCref = visitConversionOperatorMemberCref;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during cref parameter list visits.
        /// </summary>
        /// <param name="visitCrefParameterList">The function to call during cref parameter list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitCrefParameterList(Func<CSharpSyntaxRewriter, CrefParameterListSyntax, SyntaxNode> visitCrefParameterList = default)
        {
            _visitCrefParameterList = visitCrefParameterList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during cref bracketed parameter list visits.
        /// </summary>
        /// <param name="visitCrefBracketedParameterList">The function to call during cref bracketed parameter list visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitCrefBracketedParameterList(Func<CSharpSyntaxRewriter, CrefBracketedParameterListSyntax, SyntaxNode> visitCrefBracketedParameterList = default)
        {
            _visitCrefBracketedParameterList = visitCrefBracketedParameterList;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during cref parameter visits.
        /// </summary>
        /// <param name="visitCrefParameter">The function to call during cref parameter visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitCrefParameter(Func<CSharpSyntaxRewriter, CrefParameterSyntax, SyntaxNode> visitCrefParameter = default)
        {
            _visitCrefParameter = visitCrefParameter;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during XML element visits.
        /// </summary>
        /// <param name="visitXmlElement">The function to call during XML element visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitXmlElement(Func<CSharpSyntaxRewriter, XmlElementSyntax, SyntaxNode> visitXmlElement = default)
        {
            _visitXmlElement = visitXmlElement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during XML element start tag visits.
        /// </summary>
        /// <param name="visitXmlElementStartTag">The function to call during XML element start tag visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitXmlElementStartTag(Func<CSharpSyntaxRewriter, XmlElementStartTagSyntax, SyntaxNode> visitXmlElementStartTag = default)
        {
            _visitXmlElementStartTag = visitXmlElementStartTag;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during XML element end tag visits.
        /// </summary>
        /// <param name="visitXmlElementEndTag">The function to call during XML element end tag visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitXmlElementEndTag(Func<CSharpSyntaxRewriter, XmlElementEndTagSyntax, SyntaxNode> visitXmlElementEndTag = default)
        {
            _visitXmlElementEndTag = visitXmlElementEndTag;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during XML empty element visits.
        /// </summary>
        /// <param name="visitXmlEmptyElement">The function to call during XML empty element visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitXmlEmptyElement(Func<CSharpSyntaxRewriter, XmlEmptyElementSyntax, SyntaxNode> visitXmlEmptyElement = default)
        {
            _visitXmlEmptyElement = visitXmlEmptyElement;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during XML name visits.
        /// </summary>
        /// <param name="visitXmlName">The function to call during XML name visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitXmlName(Func<CSharpSyntaxRewriter, XmlNameSyntax, SyntaxNode> visitXmlName = default)
        {
            _visitXmlName = visitXmlName;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during XML prefix visits.
        /// </summary>
        /// <param name="visitXmlPrefix">The function to call during XML prefix visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitXmlPrefix(Func<CSharpSyntaxRewriter, XmlPrefixSyntax, SyntaxNode> visitXmlPrefix = default)
        {
            _visitXmlPrefix = visitXmlPrefix;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during XML text attribute visits.
        /// </summary>
        /// <param name="visitXmlTextAttribute">The function to call during XML text attribute visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitXmlTextAttribute(Func<CSharpSyntaxRewriter, XmlTextAttributeSyntax, SyntaxNode> visitXmlTextAttribute = default)
        {
            _visitXmlTextAttribute = visitXmlTextAttribute;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during XML cref attribute visits.
        /// </summary>
        /// <param name="visitXmlCrefAttribute">The function to call during XML cref attribute visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitXmlCrefAttribute(Func<CSharpSyntaxRewriter, XmlCrefAttributeSyntax, SyntaxNode> visitXmlCrefAttribute = default)
        {
            _visitXmlCrefAttribute = visitXmlCrefAttribute;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during XML name attribute visits.
        /// </summary>
        /// <param name="visitXmlNameAttribute">The function to call during XML name attribute visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitXmlNameAttribute(Func<CSharpSyntaxRewriter, XmlNameAttributeSyntax, SyntaxNode> visitXmlNameAttribute = default)
        {
            _visitXmlNameAttribute = visitXmlNameAttribute;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during XML text visits.
        /// </summary>
        /// <param name="visitXmlText">The function to call during XML text visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitXmlText(Func<CSharpSyntaxRewriter, XmlTextSyntax, SyntaxNode> visitXmlText = default)
        {
            _visitXmlText = visitXmlText;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during XML CDATA section visits.
        /// </summary>
        /// <param name="visitXmlCDataSection">The function to call during XML CDATA section visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitXmlCDataSection(Func<CSharpSyntaxRewriter, XmlCDataSectionSyntax, SyntaxNode> visitXmlCDataSection = default)
        {
            _visitXmlCDataSection = visitXmlCDataSection;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during XML processing instruction visits.
        /// </summary>
        /// <param name="visitXmlProcessingInstruction">The function to call during XML processing instruction visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitXmlProcessingInstruction(Func<CSharpSyntaxRewriter, XmlProcessingInstructionSyntax, SyntaxNode> visitXmlProcessingInstruction = default)
        {
            _visitXmlProcessingInstruction = visitXmlProcessingInstruction;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during XML comment visits.
        /// </summary>
        /// <param name="visitXmlComment">The function to call during XML comment visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitXmlComment(Func<CSharpSyntaxRewriter, XmlCommentSyntax, SyntaxNode> visitXmlComment = default)
        {
            _visitXmlComment = visitXmlComment;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during if directive trivia visits.
        /// </summary>
        /// <param name="visitIfDirectiveTrivia">The function to call during if directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitIfDirectiveTrivia(Func<CSharpSyntaxRewriter, IfDirectiveTriviaSyntax, SyntaxNode> visitIfDirectiveTrivia = default)
        {
            _visitIfDirectiveTrivia = visitIfDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during elif directive trivia visits.
        /// </summary>
        /// <param name="visitElifDirectiveTrivia">The function to call during elif directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitElifDirectiveTrivia(Func<CSharpSyntaxRewriter, ElifDirectiveTriviaSyntax, SyntaxNode> visitElifDirectiveTrivia = default)
        {
            _visitElifDirectiveTrivia = visitElifDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during else directive trivia visits.
        /// </summary>
        /// <param name="visitElseDirectiveTrivia">The function to call during else directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitElseDirectiveTrivia(Func<CSharpSyntaxRewriter, ElseDirectiveTriviaSyntax, SyntaxNode> visitElseDirectiveTrivia = default)
        {
            _visitElseDirectiveTrivia = visitElseDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during endif directive trivia visits.
        /// </summary>
        /// <param name="visitEndIfDirectiveTrivia">The function to call during endif directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitEndIfDirectiveTrivia(Func<CSharpSyntaxRewriter, EndIfDirectiveTriviaSyntax, SyntaxNode> visitEndIfDirectiveTrivia = default)
        {
            _visitEndIfDirectiveTrivia = visitEndIfDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during region directive trivia visits.
        /// </summary>
        /// <param name="visitRegionDirectiveTrivia">The function to call during region directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitRegionDirectiveTrivia(Func<CSharpSyntaxRewriter, RegionDirectiveTriviaSyntax, SyntaxNode> visitRegionDirectiveTrivia = default)
        {
            _visitRegionDirectiveTrivia = visitRegionDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during end region directive trivia visits.
        /// </summary>
        /// <param name="visitEndRegionDirectiveTrivia">The function to call during end region directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitEndRegionDirectiveTrivia(Func<CSharpSyntaxRewriter, EndRegionDirectiveTriviaSyntax, SyntaxNode> visitEndRegionDirectiveTrivia = default)
        {
            _visitEndRegionDirectiveTrivia = visitEndRegionDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during error directive trivia visits.
        /// </summary>
        /// <param name="visitErrorDirectiveTrivia">The function to call during error directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitErrorDirectiveTrivia(Func<CSharpSyntaxRewriter, ErrorDirectiveTriviaSyntax, SyntaxNode> visitErrorDirectiveTrivia = default)
        {
            _visitErrorDirectiveTrivia = visitErrorDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during warning directive trivia visits.
        /// </summary>
        /// <param name="visitWarningDirectiveTrivia">The function to call during warning directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitWarningDirectiveTrivia(Func<CSharpSyntaxRewriter, WarningDirectiveTriviaSyntax, SyntaxNode> visitWarningDirectiveTrivia = default)
        {
            _visitWarningDirectiveTrivia = visitWarningDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during bad directive trivia visits.
        /// </summary>
        /// <param name="visitBadDirectiveTrivia">The function to call during bad directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitBadDirectiveTrivia(Func<CSharpSyntaxRewriter, BadDirectiveTriviaSyntax, SyntaxNode> visitBadDirectiveTrivia = default)
        {
            _visitBadDirectiveTrivia = visitBadDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during define directive trivia visits.
        /// </summary>
        /// <param name="visitDefineDirectiveTrivia">The function to call during define directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitDefineDirectiveTrivia(Func<CSharpSyntaxRewriter, DefineDirectiveTriviaSyntax, SyntaxNode> visitDefineDirectiveTrivia = default)
        {
            _visitDefineDirectiveTrivia = visitDefineDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during undef directive trivia visits.
        /// </summary>
        /// <param name="visitUndefDirectiveTrivia">The function to call during undef directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitUndefDirectiveTrivia(Func<CSharpSyntaxRewriter, UndefDirectiveTriviaSyntax, SyntaxNode> visitUndefDirectiveTrivia = default)
        {
            _visitUndefDirectiveTrivia = visitUndefDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during line directive trivia visits.
        /// </summary>
        /// <param name="visitLineDirectiveTrivia">The function to call during line directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitLineDirectiveTrivia(Func<CSharpSyntaxRewriter, LineDirectiveTriviaSyntax, SyntaxNode> visitLineDirectiveTrivia = default)
        {
            _visitLineDirectiveTrivia = visitLineDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during line directive position visits.
        /// </summary>
        /// <param name="visitLineDirectivePosition">The function to call during line directive position visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitLineDirectivePosition(Func<CSharpSyntaxRewriter, LineDirectivePositionSyntax, SyntaxNode> visitLineDirectivePosition = default)
        {
            _visitLineDirectivePosition = visitLineDirectivePosition;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during line span directive trivia visits.
        /// </summary>
        /// <param name="visitLineSpanDirectiveTrivia">The function to call during line span directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitLineSpanDirectiveTrivia(Func<CSharpSyntaxRewriter, LineSpanDirectiveTriviaSyntax, SyntaxNode> visitLineSpanDirectiveTrivia = default)
        {
            _visitLineSpanDirectiveTrivia = visitLineSpanDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during pragma warning directive trivia visits.
        /// </summary>
        /// <param name="visitPragmaWarningDirectiveTrivia">The function to call during pragma warning directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitPragmaWarningDirectiveTrivia(Func<CSharpSyntaxRewriter, PragmaWarningDirectiveTriviaSyntax, SyntaxNode> visitPragmaWarningDirectiveTrivia = default)
        {
            _visitPragmaWarningDirectiveTrivia = visitPragmaWarningDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during pragma checksum directive trivia visits.
        /// </summary>
        /// <param name="visitPragmaChecksumDirectiveTrivia">The function to call during pragma checksum directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitPragmaChecksumDirectiveTrivia(Func<CSharpSyntaxRewriter, PragmaChecksumDirectiveTriviaSyntax, SyntaxNode> visitPragmaChecksumDirectiveTrivia = default)
        {
            _visitPragmaChecksumDirectiveTrivia = visitPragmaChecksumDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during reference directive trivia visits.
        /// </summary>
        /// <param name="visitReferenceDirectiveTrivia">The function to call during reference directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitReferenceDirectiveTrivia(Func<CSharpSyntaxRewriter, ReferenceDirectiveTriviaSyntax, SyntaxNode> visitReferenceDirectiveTrivia = default)
        {
            _visitReferenceDirectiveTrivia = visitReferenceDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during load directive trivia visits.
        /// </summary>
        /// <param name="visitLoadDirectiveTrivia">The function to call during load directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitLoadDirectiveTrivia(Func<CSharpSyntaxRewriter, LoadDirectiveTriviaSyntax, SyntaxNode> visitLoadDirectiveTrivia = default)
        {
            _visitLoadDirectiveTrivia = visitLoadDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during shebang directive trivia visits.
        /// </summary>
        /// <param name="visitShebangDirectiveTrivia">The function to call during shebang directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitShebangDirectiveTrivia(Func<CSharpSyntaxRewriter, ShebangDirectiveTriviaSyntax, SyntaxNode> visitShebangDirectiveTrivia = default)
        {
            _visitShebangDirectiveTrivia = visitShebangDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Sets a function to be called during nullable directive trivia visits.
        /// </summary>
        /// <param name="visitNullableDirectiveTrivia">The function to call during nullable directive trivia visits.</param>
        /// <returns>The current <see cref="FluentCSharpSyntaxRewriter"/> instance.</returns>
        public virtual FluentCSharpSyntaxRewriter WithVisitNullableDirectiveTrivia(Func<CSharpSyntaxRewriter, NullableDirectiveTriviaSyntax, SyntaxNode> visitNullableDirectiveTrivia = default)
        {
            _visitNullableDirectiveTrivia = visitNullableDirectiveTrivia;
            return this;
        }

        /// <summary>
        /// Provides a default visit implementation for syntax nodes.
        /// </summary>
        /// <param name="node">The syntax node to visit.</param>
        /// <returns>The visited syntax node, possibly modified.</returns>
        public override SyntaxNode DefaultVisit(SyntaxNode node)
        {
            if (node == null)
                return null;
            if (_defaultVisit != null)
                node = _defaultVisit.Invoke(this, node);
            return base.DefaultVisit(node);
        }

        // TODO: Visit [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull("node")]

        /// <summary>
        /// Visits the specified syntax node.
        /// </summary>
        /// <param name="node">The syntax node to visit.</param>
        /// <returns>The visited syntax node, possibly modified.</returns>
        public override SyntaxNode Visit(SyntaxNode node)
        {
            if (_visit != null)
                node = _visit.Invoke(this, node);
            return base.Visit(node);
        }

        /// <summary>
        /// Visits the specified list of syntax tokens.
        /// </summary>
        /// <param name="list">The list of syntax tokens to visit.</param>
        /// <returns>The visited list of syntax tokens, possibly modified.</returns>
        public override SyntaxTokenList VisitList(SyntaxTokenList list)
        {
            if (_visitTokenList != null)
                list = _visitTokenList.Invoke(this, list);
            return base.VisitList(list);
        }

        /// <summary>
        /// Visits the specified list of syntax trivia.
        /// </summary>
        /// <param name="list">The list of syntax trivia to visit.</param>
        /// <returns>The visited list of syntax trivia, possibly modified.</returns>
        public override SyntaxTriviaList VisitList(SyntaxTriviaList list)
        {
            if (_visitTriviaList != null)
                list = _visitTriviaList.Invoke(this, list);
            return base.VisitList(list);
        }

        /// <summary>
        /// Visits the specified syntax trivia element.
        /// </summary>
        /// <param name="element">The syntax trivia element to visit.</param>
        /// <returns>The visited syntax trivia element, possibly modified.</returns>
        public override SyntaxTrivia VisitListElement(SyntaxTrivia element)
        {
            if (_visitListElement != null)
                element = _visitListElement.Invoke(this, element);
            return base.VisitListElement(element);
        }

        /// <summary>
        /// Visits the specified syntax token that serves as a list separator.
        /// </summary>
        /// <param name="separator">The syntax token serving as a list separator.</param>
        /// <returns>The visited syntax token, possibly modified.</returns>
        public override SyntaxToken VisitListSeparator(SyntaxToken separator)
        {
            if (_visitListSeparator != null)
                separator = _visitListSeparator.Invoke(this, separator);
            return base.VisitListSeparator(separator);
        }

        /// <summary>
        /// Visits the specified syntax node that is an element of a list.
        /// </summary>
        /// <typeparam name="TNode">The type of the syntax node.</typeparam>
        /// <param name="node">The syntax node to visit.</param>
        /// <returns>The visited syntax node, possibly modified.</returns>
        public override TNode VisitListElement<TNode>(TNode node)
        {
            if (!_visitListElementNode.TryGetValue(typeof(TNode), out var @delegate))
                return base.VisitListElement(node);
            if (@delegate is Func<CSharpSyntaxRewriter, TNode, TNode> visitListElementNode)
                node = visitListElementNode.Invoke(this, node);
            return base.VisitListElement(node);
        }

        /// <summary>
        /// Visits the specified separated syntax list of nodes.
        /// </summary>
        /// <typeparam name="TNode">The type of the syntax nodes in the list.</typeparam>
        /// <param name="list">The separated syntax list of nodes to visit.</param>
        /// <returns>The visited separated syntax list of nodes, possibly modified.</returns>
        public override SeparatedSyntaxList<TNode> VisitList<TNode>(SeparatedSyntaxList<TNode> list)
        {
            if (!_visitListSeparatedList.TryGetValue(typeof(TNode), out var @delegate))
                return base.VisitList(list);
            if (@delegate is Func<CSharpSyntaxRewriter, SeparatedSyntaxList<TNode>, SeparatedSyntaxList<TNode>> visitListSeparatedList)
                list = visitListSeparatedList.Invoke(this, list);
            return base.VisitList(list);
        }

        /// <summary>
        /// Visits the specified syntax list of nodes.
        /// </summary>
        /// <typeparam name="TNode">The type of the syntax nodes in the list.</typeparam>
        /// <param name="list">The syntax list of nodes to visit.</param>
        /// <returns>The visited syntax list of nodes, possibly modified.</returns>
        public override SyntaxList<TNode> VisitList<TNode>(SyntaxList<TNode> list)
        {
            if (!_visitListNode.TryGetValue(typeof(TNode), out var @delegate))
                return base.VisitList(list);
            if (@delegate is Func<CSharpSyntaxRewriter, SyntaxList<TNode>, SyntaxList<TNode>> visitListNode)
                list = visitListNode.Invoke(this, list);
            return base.VisitList(list);
        }

        /// <summary>
        /// Visits the specified syntax token.
        /// </summary>
        /// <param name="token">The syntax token to visit.</param>
        /// <returns>The visited syntax token, possibly modified.</returns>
        public override SyntaxToken VisitToken(SyntaxToken token)
        {
            if (_visitToken != null)
                token = _visitToken.Invoke(this, token);
            return base.VisitToken(token);
        }

        /// <summary>
        /// Visits the specified syntax trivia.
        /// </summary>
        /// <param name="node">The syntax trivia to visit.</param>
        /// <returns>The visited syntax trivia, possibly modified.</returns>
        public override SyntaxTrivia VisitTrivia(SyntaxTrivia node)
        {
            if (_visitTrivia != null)
                node = _visitTrivia.Invoke(this, node);
            return base.VisitTrivia(node);
        }

        /// <summary>
        /// Visits the specified identifier name syntax node.
        /// </summary>
        /// <param name="node">The identifier name syntax node to visit.</param>
        /// <returns>The visited identifier name syntax node, possibly modified.</returns>
        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (_visitIdentifierName != null)
            {
                var visit = _visitIdentifierName.Invoke(this, node);
                return (visit is IdentifierNameSyntax newNode) ? base.VisitIdentifierName(newNode) : visit;
            }
            return base.VisitIdentifierName(node);
        }

        /// <summary>
        /// Visits the specified qualified name syntax node.
        /// </summary>
        /// <param name="node">The qualified name syntax node to visit.</param>
        /// <returns>The visited qualified name syntax node, possibly modified.</returns>
        public override SyntaxNode VisitQualifiedName(QualifiedNameSyntax node)
        {
            if (_visitQualifiedName != null)
            {
                var visit = _visitQualifiedName.Invoke(this, node);
                return (visit is QualifiedNameSyntax newNode) ? base.VisitQualifiedName(newNode) : visit;
            }
            return base.VisitQualifiedName(node);
        }

        /// <summary>
        /// Visits the specified generic name syntax node.
        /// </summary>
        /// <param name="node">The generic name syntax node to visit.</param>
        /// <returns>The visited generic name syntax node, possibly modified.</returns>
        public override SyntaxNode VisitGenericName(GenericNameSyntax node)
        {
            if (_visitGenericName != null)
            {
                var visit = _visitGenericName.Invoke(this, node);
                return (visit is GenericNameSyntax newNode) ? base.VisitGenericName(newNode) : visit;
            }
            return base.VisitGenericName(node);
        }

        /// <summary>
        /// Visits the specified type argument list syntax node.
        /// </summary>
        /// <param name="node">The type argument list syntax node to visit.</param>
        /// <returns>The visited type argument list syntax node, possibly modified.</returns>
        public override SyntaxNode VisitTypeArgumentList(TypeArgumentListSyntax node)
        {
            if (_visitTypeArgumentList != null)
            {
                var visit = _visitTypeArgumentList.Invoke(this, node);
                return (visit is TypeArgumentListSyntax newNode) ? base.VisitTypeArgumentList(newNode) : visit;
            }
            return base.VisitTypeArgumentList(node);
        }

        /// <summary>
        /// Visits the specified alias qualified name syntax node.
        /// </summary>
        /// <param name="node">The alias qualified name syntax node to visit.</param>
        /// <returns>The visited alias qualified name syntax node, possibly modified.</returns>
        public override SyntaxNode VisitAliasQualifiedName(AliasQualifiedNameSyntax node)
        {
            if (_visitAliasQualifiedName != null)
            {
                var visit = _visitAliasQualifiedName.Invoke(this, node);
                return (visit is AliasQualifiedNameSyntax newNode) ? base.VisitAliasQualifiedName(newNode) : visit;
            }
            return base.VisitAliasQualifiedName(node);
        }

        /// <summary>
        /// Visits the specified predefined type syntax node.
        /// </summary>
        /// <param name="node">The predefined type syntax node to visit.</param>
        /// <returns>The visited predefined type syntax node, possibly modified.</returns>
        public override SyntaxNode VisitPredefinedType(PredefinedTypeSyntax node)
        {
            if (_visitPredefinedType != null)
            {
                var visit = _visitPredefinedType.Invoke(this, node);
                return (visit is PredefinedTypeSyntax newNode) ? base.VisitPredefinedType(newNode) : visit;
            }
            return base.VisitPredefinedType(node);
        }

        /// <summary>
        /// Visits the specified array type syntax node.
        /// </summary>
        /// <param name="node">The array type syntax node to visit.</param>
        /// <returns>The visited array type syntax node, possibly modified.</returns>
        public override SyntaxNode VisitArrayType(ArrayTypeSyntax node)
        {
            if (_visitArrayType != null)
            {
                var visit = _visitArrayType.Invoke(this, node);
                return (visit is ArrayTypeSyntax newNode) ? base.VisitArrayType(newNode) : visit;
            }
            return base.VisitArrayType(node);
        }

        /// <summary>
        /// Visits the specified array rank specifier syntax node.
        /// </summary>
        /// <param name="node">The array rank specifier syntax node to visit.</param>
        /// <returns>The visited array rank specifier syntax node, possibly modified.</returns>
        public override SyntaxNode VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node)
        {
            if (_visitArrayRankSpecifier != null)
            {
                var visit = _visitArrayRankSpecifier.Invoke(this, node);
                return (visit is ArrayRankSpecifierSyntax newNode) ? base.VisitArrayRankSpecifier(newNode) : visit;
            }
            return base.VisitArrayRankSpecifier(node);
        }

        /// <summary>
        /// Visits the specified pointer type syntax node.
        /// </summary>
        /// <param name="node">The pointer type syntax node to visit.</param>
        /// <returns>The visited pointer type syntax node, possibly modified.</returns>
        public override SyntaxNode VisitPointerType(PointerTypeSyntax node)
        {
            if (_visitPointerType != null)
            {
                var visit = _visitPointerType.Invoke(this, node);
                return (visit is PointerTypeSyntax newNode) ? base.VisitPointerType(newNode) : visit;
            }
            return base.VisitPointerType(node);
        }

        /// <summary>
        /// Visits the specified function pointer type syntax node.
        /// </summary>
        /// <param name="node">The function pointer type syntax node to visit.</param>
        /// <returns>The visited function pointer type syntax node, possibly modified.</returns>
        public override SyntaxNode VisitFunctionPointerType(FunctionPointerTypeSyntax node)
        {
            if (_visitFunctionPointerType != null)
            {
                var visit = _visitFunctionPointerType.Invoke(this, node);
                return (visit is FunctionPointerTypeSyntax newNode) ? base.VisitFunctionPointerType(newNode) : visit;
            }
            return base.VisitFunctionPointerType(node);
        }

        /// <summary>
        /// Visits the specified function pointer parameter list syntax node.
        /// </summary>
        /// <param name="node">The function pointer parameter list syntax node to visit.</param>
        /// <returns>The visited function pointer parameter list syntax node, possibly modified.</returns>
        public override SyntaxNode VisitFunctionPointerParameterList(FunctionPointerParameterListSyntax node)
        {
            if (_visitFunctionPointerParameterList != null)
            {
                var visit = _visitFunctionPointerParameterList.Invoke(this, node);
                return (visit is FunctionPointerParameterListSyntax newNode) ? base.VisitFunctionPointerParameterList(newNode) : visit;
            }
            return base.VisitFunctionPointerParameterList(node);
        }

        /// <summary>
        /// Visits the specified function pointer calling convention syntax node.
        /// </summary>
        /// <param name="node">The function pointer calling convention syntax node to visit.</param>
        /// <returns>The visited function pointer calling convention syntax node, possibly modified.</returns>
        public override SyntaxNode VisitFunctionPointerCallingConvention(FunctionPointerCallingConventionSyntax node)
        {
            if (_visitFunctionPointerCallingConvention != null)
            {
                var visit = _visitFunctionPointerCallingConvention.Invoke(this, node);
                return (visit is FunctionPointerCallingConventionSyntax newNode) ? base.VisitFunctionPointerCallingConvention(newNode) : visit;
            }
            return base.VisitFunctionPointerCallingConvention(node);
        }

        /// <summary>
        /// Visits the specified function pointer unmanaged calling convention list syntax node.
        /// </summary>
        /// <param name="node">The function pointer unmanaged calling convention list syntax node to visit.</param>
        /// <returns>The visited function pointer unmanaged calling convention list syntax node, possibly modified.</returns>
        public override SyntaxNode VisitFunctionPointerUnmanagedCallingConventionList(FunctionPointerUnmanagedCallingConventionListSyntax node)
        {
            if (_visitFunctionPointerUnmanagedCallingConventionList != null)
            {
                var visit = _visitFunctionPointerUnmanagedCallingConventionList.Invoke(this, node);
                return (visit is FunctionPointerUnmanagedCallingConventionListSyntax newNode) ? base.VisitFunctionPointerUnmanagedCallingConventionList(newNode) : visit;
            }
            return base.VisitFunctionPointerUnmanagedCallingConventionList(node);
        }

        /// <summary>
        /// Visits the specified function pointer unmanaged calling convention syntax node.
        /// </summary>
        /// <param name="node">The function pointer unmanaged calling convention syntax node to visit.</param>
        /// <returns>The visited function pointer unmanaged calling convention syntax node, possibly modified.</returns>
        public override SyntaxNode VisitFunctionPointerUnmanagedCallingConvention(FunctionPointerUnmanagedCallingConventionSyntax node)
        {
            if (_visitFunctionPointerUnmanagedCallingConvention != null)
            {
                var visit = _visitFunctionPointerUnmanagedCallingConvention.Invoke(this, node);
                return (visit is FunctionPointerUnmanagedCallingConventionSyntax newNode) ? base.VisitFunctionPointerUnmanagedCallingConvention(newNode) : visit;
            }
            return base.VisitFunctionPointerUnmanagedCallingConvention(node);
        }

        /// <summary>
        /// Visits the specified nullable type syntax node.
        /// </summary>
        /// <param name="node">The nullable type syntax node to visit.</param>
        /// <returns>The visited nullable type syntax node, possibly modified.</returns>
        public override SyntaxNode VisitNullableType(NullableTypeSyntax node)
        {
            if (_visitNullableType != null)
            {
                var visit = _visitNullableType.Invoke(this, node);
                return (visit is NullableTypeSyntax newNode) ? base.VisitNullableType(newNode) : visit;
            }
            return base.VisitNullableType(node);
        }

        /// <summary>
        /// Visits the specified tuple type syntax node.
        /// </summary>
        /// <param name="node">The tuple type syntax node to visit.</param>
        /// <returns>The visited tuple type syntax node, possibly modified.</returns>
        public override SyntaxNode VisitTupleType(TupleTypeSyntax node)
        {
            if (_visitTupleType != null)
            {
                var visit = _visitTupleType.Invoke(this, node);
                return (visit is TupleTypeSyntax newNode) ? base.VisitTupleType(newNode) : visit;
            }
            return base.VisitTupleType(node);
        }

        /// <summary>
        /// Visits the specified tuple element syntax node.
        /// </summary>
        /// <param name="node">The tuple element syntax node to visit.</param>
        /// <returns>The visited tuple element syntax node, possibly modified.</returns>
        public override SyntaxNode VisitTupleElement(TupleElementSyntax node)
        {
            if (_visitTupleElement != null)
            {
                var visit = _visitTupleElement.Invoke(this, node);
                return (visit is TupleElementSyntax newNode) ? base.VisitTupleElement(newNode) : visit;
            }
            return base.VisitTupleElement(node);
        }

        /// <summary>
        /// Visits the specified omitted type argument syntax node.
        /// </summary>
        /// <param name="node">The omitted type argument syntax node to visit.</param>
        /// <returns>The visited omitted type argument syntax node, possibly modified.</returns>
        public override SyntaxNode VisitOmittedTypeArgument(OmittedTypeArgumentSyntax node)
        {
            if (_visitOmittedTypeArgument != null)
            {
                var visit = _visitOmittedTypeArgument.Invoke(this, node);
                return (visit is OmittedTypeArgumentSyntax newNode) ? base.VisitOmittedTypeArgument(newNode) : visit;
            }
            return base.VisitOmittedTypeArgument(node);
        }

        /// <summary>
        /// Visits the specified ref type syntax node.
        /// </summary>
        /// <param name="node">The ref type syntax node to visit.</param>
        /// <returns>The visited ref type syntax node, possibly modified.</returns>
        public override SyntaxNode VisitRefType(RefTypeSyntax node)
        {
            if (_visitRefType != null)
            {
                var visit = _visitRefType.Invoke(this, node);
                return (visit is RefTypeSyntax newNode) ? base.VisitRefType(newNode) : visit;
            }
            return base.VisitRefType(node);
        }

        /// <summary>
        /// Visits the specified scoped type syntax node.
        /// </summary>
        /// <param name="node">The scoped type syntax node to visit.</param>
        /// <returns>The visited scoped type syntax node, possibly modified.</returns>
        public override SyntaxNode VisitScopedType(ScopedTypeSyntax node)
        {
            if (_visitScopedType != null)
            {
                var visit = _visitScopedType.Invoke(this, node);
                return (visit is ScopedTypeSyntax newNode) ? base.VisitScopedType(newNode) : visit;
            }
            return base.VisitScopedType(node);
        }

        /// <summary>
        /// Visits the specified parenthesized expression syntax node.
        /// </summary>
        /// <param name="node">The parenthesized expression syntax node to visit.</param>
        /// <returns>The visited parenthesized expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            if (_visitParenthesizedExpression != null)
            {
                var visit = _visitParenthesizedExpression.Invoke(this, node);
                return (visit is ParenthesizedExpressionSyntax newNode) ? base.VisitParenthesizedExpression(newNode) : visit;
            }
            return base.VisitParenthesizedExpression(node);
        }

        /// <summary>
        /// Visits the specified tuple expression syntax node.
        /// </summary>
        /// <param name="node">The tuple expression syntax node to visit.</param>
        /// <returns>The visited tuple expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitTupleExpression(TupleExpressionSyntax node)
        {
            if (_visitTupleExpression != null)
            {
                var visit = _visitTupleExpression.Invoke(this, node);
                return (visit is TupleExpressionSyntax newNode) ? base.VisitTupleExpression(newNode) : visit;
            }
            return base.VisitTupleExpression(node);
        }

        /// <summary>
        /// Visits the specified prefix unary expression syntax node.
        /// </summary>
        /// <param name="node">The prefix unary expression syntax node to visit.</param>
        /// <returns>The visited prefix unary expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            if (_visitPrefixUnaryExpression != null)
            {
                var visit = _visitPrefixUnaryExpression.Invoke(this, node);
                return (visit is PrefixUnaryExpressionSyntax newNode) ? base.VisitPrefixUnaryExpression(newNode) : visit;
            }
            return base.VisitPrefixUnaryExpression(node);
        }

        /// <summary>
        /// Visits the specified await expression syntax node.
        /// </summary>
        /// <param name="node">The await expression syntax node to visit.</param>
        /// <returns>The visited await expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitAwaitExpression(AwaitExpressionSyntax node)
        {
            if (_visitAwaitExpression != null)
            {
                var visit = _visitAwaitExpression.Invoke(this, node);
                return (visit is AwaitExpressionSyntax newNode) ? base.VisitAwaitExpression(newNode) : visit;
            }
            return base.VisitAwaitExpression(node);
        }

        /// <summary>
        /// Visits the specified postfix unary expression syntax node.
        /// </summary>
        /// <param name="node">The postfix unary expression syntax node to visit.</param>
        /// <returns>The visited postfix unary expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            if (_visitPostfixUnaryExpression != null)
            {
                var visit = _visitPostfixUnaryExpression.Invoke(this, node);
                return (visit is PostfixUnaryExpressionSyntax newNode) ? base.VisitPostfixUnaryExpression(newNode) : visit;
            }
            return base.VisitPostfixUnaryExpression(node);
        }

        /// <summary>
        /// Visits the specified member access expression syntax node.
        /// </summary>
        /// <param name="node">The member access expression syntax node to visit.</param>
        /// <returns>The visited member access expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (_visitMemberAccessExpression != null)
            {
                var visit = _visitMemberAccessExpression.Invoke(this, node);
                return (visit is MemberAccessExpressionSyntax newNode) ? base.VisitMemberAccessExpression(newNode) : visit;
            }
            return base.VisitMemberAccessExpression(node);
        }

        /// <summary>
        /// Visits the specified conditional access expression syntax node.
        /// </summary>
        /// <param name="node">The conditional access expression syntax node to visit.</param>
        /// <returns>The visited conditional access expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
        {
            if (_visitConditionalAccessExpression != null)
            {
                var visit = _visitConditionalAccessExpression.Invoke(this, node);
                return (visit is ConditionalAccessExpressionSyntax newNode) ? base.VisitConditionalAccessExpression(newNode) : visit;
            }
            return base.VisitConditionalAccessExpression(node);
        }

        /// <summary>
        /// Visits the specified member binding expression syntax node.
        /// </summary>
        /// <param name="node">The member binding expression syntax node to visit.</param>
        /// <returns>The visited member binding expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitMemberBindingExpression(MemberBindingExpressionSyntax node)
        {
            if (_visitMemberBindingExpression != null)
            {
                var visit = _visitMemberBindingExpression.Invoke(this, node);
                return (visit is MemberBindingExpressionSyntax newNode) ? base.VisitMemberBindingExpression(newNode) : visit;
            }
            return base.VisitMemberBindingExpression(node);
        }

        /// <summary>
        /// Visits the specified element binding expression syntax node.
        /// </summary>
        /// <param name="node">The element binding expression syntax node to visit.</param>
        /// <returns>The visited element binding expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitElementBindingExpression(ElementBindingExpressionSyntax node)
        {
            if (_visitElementBindingExpression != null)
            {
                var visit = _visitElementBindingExpression.Invoke(this, node);
                return (visit is ElementBindingExpressionSyntax newNode) ? base.VisitElementBindingExpression(newNode) : visit;
            }
            return base.VisitElementBindingExpression(node);
        }

        /// <summary>
        /// Visits the specified range expression syntax node.
        /// </summary>
        /// <param name="node">The range expression syntax node to visit.</param>
        /// <returns>The visited range expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitRangeExpression(RangeExpressionSyntax node)
        {
            if (_visitRangeExpression != null)
            {
                var visit = _visitRangeExpression.Invoke(this, node);
                return (visit is RangeExpressionSyntax newNode) ? base.VisitRangeExpression(newNode) : visit;
            }
            return base.VisitRangeExpression(node);
        }

        /// <summary>
        /// Visits the specified implicit element access syntax node.
        /// </summary>
        /// <param name="node">The implicit element access syntax node to visit.</param>
        /// <returns>The visited implicit element access syntax node, possibly modified.</returns>
        public override SyntaxNode VisitImplicitElementAccess(ImplicitElementAccessSyntax node)
        {
            if (_visitImplicitElementAccess != null)
            {
                var visit = _visitImplicitElementAccess.Invoke(this, node);
                return (visit is ImplicitElementAccessSyntax newNode) ? base.VisitImplicitElementAccess(newNode) : visit;
            }
            return base.VisitImplicitElementAccess(node);
        }

        /// <summary>
        /// Visits the specified binary expression syntax node.
        /// </summary>
        /// <param name="node">The binary expression syntax node to visit.</param>
        /// <returns>The visited binary expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            if (_visitBinaryExpression != null)
            {
                var visit = _visitBinaryExpression.Invoke(this, node);
                return (visit is BinaryExpressionSyntax newNode) ? base.VisitBinaryExpression(newNode) : visit;
            }
            return base.VisitBinaryExpression(node);
        }

        /// <summary>
        /// Visits the specified assignment expression syntax node.
        /// </summary>
        /// <param name="node">The assignment expression syntax node to visit.</param>
        /// <returns>The visited assignment expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            if (_visitAssignmentExpression != null)
            {
                var visit = _visitAssignmentExpression.Invoke(this, node);
                return (visit is AssignmentExpressionSyntax newNode) ? base.VisitAssignmentExpression(newNode) : visit;
            }
            return base.VisitAssignmentExpression(node);
        }

        /// <summary>
        /// Visits the specified conditional expression syntax node.
        /// </summary>
        /// <param name="node">The conditional expression syntax node to visit.</param>
        /// <returns>The visited conditional expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            if (_visitConditionalExpression != null)
            {
                var visit = _visitConditionalExpression.Invoke(this, node);
                return (visit is ConditionalExpressionSyntax newNode) ? base.VisitConditionalExpression(newNode) : visit;
            }
            return base.VisitConditionalExpression(node);
        }

        /// <summary>
        /// Visits the specified 'this' expression syntax node.
        /// </summary>
        /// <param name="node">The 'this' expression syntax node to visit.</param>
        /// <returns>The visited 'this' expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitThisExpression(ThisExpressionSyntax node)
        {
            if (_visitThisExpression != null)
            {
                var visit = _visitThisExpression.Invoke(this, node);
                return (visit is ThisExpressionSyntax newNode) ? base.VisitThisExpression(newNode) : visit;
            }
            return base.VisitThisExpression(node);
        }

        /// <summary>
        /// Visits the specified 'base' expression syntax node.
        /// </summary>
        /// <param name="node">The 'base' expression syntax node to visit.</param>
        /// <returns>The visited 'base' expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitBaseExpression(BaseExpressionSyntax node)
        {
            if (_visitBaseExpression != null)
            {
                var visit = _visitBaseExpression.Invoke(this, node);
                return (visit is BaseExpressionSyntax newNode) ? base.VisitBaseExpression(newNode) : visit;
            }
            return base.VisitBaseExpression(node);
        }

        /// <summary>
        /// Visits the specified literal expression syntax node.
        /// </summary>
        /// <param name="node">The literal expression syntax node to visit.</param>
        /// <returns>The visited literal expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            if (_visitLiteralExpression != null)
            {
                var visit = _visitLiteralExpression.Invoke(this, node);
                return (visit is LiteralExpressionSyntax newNode) ? base.VisitLiteralExpression(newNode) : visit;
            }
            return base.VisitLiteralExpression(node);
        }

        /// <summary>
        /// Visits the specified 'make ref' expression syntax node.
        /// </summary>
        /// <param name="node">The 'make ref' expression syntax node to visit.</param>
        /// <returns>The visited 'make ref' expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitMakeRefExpression(MakeRefExpressionSyntax node)
        {
            if (_visitMakeRefExpression != null)
            {
                var visit = _visitMakeRefExpression.Invoke(this, node);
                return (visit is MakeRefExpressionSyntax newNode) ? base.VisitMakeRefExpression(newNode) : visit;
            }
            return base.VisitMakeRefExpression(node);
        }

        /// <summary>
        /// Visits the specified ref type expression syntax node.
        /// </summary>
        /// <param name="node">The ref type expression syntax node to visit.</param>
        /// <returns>The visited ref type expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitRefTypeExpression(RefTypeExpressionSyntax node)
        {
            if (_visitRefTypeExpression != null)
            {
                var visit = _visitRefTypeExpression.Invoke(this, node);
                return (visit is RefTypeExpressionSyntax newNode) ? base.VisitRefTypeExpression(newNode) : visit;
            }
            return base.VisitRefTypeExpression(node);
        }

        /// <summary>
        /// Visits the specified ref value expression syntax node.
        /// </summary>
        /// <param name="node">The ref value expression syntax node to visit.</param>
        /// <returns>The visited ref value expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitRefValueExpression(RefValueExpressionSyntax node)
        {
            if (_visitRefValueExpression != null)
            {
                var visit = _visitRefValueExpression.Invoke(this, node);
                return (visit is RefValueExpressionSyntax newNode) ? base.VisitRefValueExpression(newNode) : visit;
            }
            return base.VisitRefValueExpression(node);
        }

        /// <summary>
        /// Visits the specified checked expression syntax node.
        /// </summary>
        /// <param name="node">The checked expression syntax node to visit.</param>
        /// <returns>The visited checked expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitCheckedExpression(CheckedExpressionSyntax node)
        {
            if (_visitCheckedExpression != null)
            {
                var visit = _visitCheckedExpression.Invoke(this, node);
                return (visit is CheckedExpressionSyntax newNode) ? base.VisitCheckedExpression(newNode) : visit;
            }
            return base.VisitCheckedExpression(node);
        }

        /// <summary>
        /// Visits the specified default expression syntax node.
        /// </summary>
        /// <param name="node">The default expression syntax node to visit.</param>
        /// <returns>The visited default expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitDefaultExpression(DefaultExpressionSyntax node)
        {
            if (_visitDefaultExpression != null)
            {
                var visit = _visitDefaultExpression.Invoke(this, node);
                return (visit is DefaultExpressionSyntax newNode) ? base.VisitDefaultExpression(newNode) : visit;
            }
            return base.VisitDefaultExpression(node);
        }

        /// <summary>
        /// Visits the specified 'typeof' expression syntax node.
        /// </summary>
        /// <param name="node">The 'typeof' expression syntax node to visit.</param>
        /// <returns>The visited 'typeof' expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitTypeOfExpression(TypeOfExpressionSyntax node)
        {
            if (_visitTypeOfExpression != null)
            {
                var visit = _visitTypeOfExpression.Invoke(this, node);
                return (visit is TypeOfExpressionSyntax newNode) ? base.VisitTypeOfExpression(newNode) : visit;
            }
            return base.VisitTypeOfExpression(node);
        }

        /// <summary>
        /// Visits the specified 'sizeof' expression syntax node.
        /// </summary>
        /// <param name="node">The 'sizeof' expression syntax node to visit.</param>
        /// <returns>The visited 'sizeof' expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitSizeOfExpression(SizeOfExpressionSyntax node)
        {
            if (_visitSizeOfExpression != null)
            {
                var visit = _visitSizeOfExpression.Invoke(this, node);
                return (visit is SizeOfExpressionSyntax newNode) ? base.VisitSizeOfExpression(newNode) : visit;
            }
            return base.VisitSizeOfExpression(node);
        }

        /// <summary>
        /// Visits the specified invocation expression syntax node.
        /// </summary>
        /// <param name="node">The invocation expression syntax node to visit.</param>
        /// <returns>The visited invocation expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (_visitInvocationExpression != null)
            {
                var visit = _visitInvocationExpression.Invoke(this, node);
                return (visit is InvocationExpressionSyntax newNode) ? base.VisitInvocationExpression(newNode) : visit;
            }
            return base.VisitInvocationExpression(node);
        }

        /// <summary>
        /// Visits the specified element access expression syntax node.
        /// </summary>
        /// <param name="node">The element access expression syntax node to visit.</param>
        /// <returns>The visited element access expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitElementAccessExpression(ElementAccessExpressionSyntax node)
        {
            if (_visitElementAccessExpression != null)
            {
                var visit = _visitElementAccessExpression.Invoke(this, node);
                return (visit is ElementAccessExpressionSyntax newNode) ? base.VisitElementAccessExpression(newNode) : visit;
            }
            return base.VisitElementAccessExpression(node);
        }

        /// <summary>
        /// Visits the specified argument list syntax node.
        /// </summary>
        /// <param name="node">The argument list syntax node to visit.</param>
        /// <returns>The visited argument list syntax node, possibly modified.</returns>
        public override SyntaxNode VisitArgumentList(ArgumentListSyntax node)
        {
            if (_visitArgumentList != null)
            {
                var visit = _visitArgumentList.Invoke(this, node);
                return (visit is ArgumentListSyntax newNode) ? base.VisitArgumentList(newNode) : visit;
            }
            return base.VisitArgumentList(node);
        }

        /// <summary>
        /// Visits the specified bracketed argument list syntax node.
        /// </summary>
        /// <param name="node">The bracketed argument list syntax node to visit.</param>
        /// <returns>The visited bracketed argument list syntax node, possibly modified.</returns>
        public override SyntaxNode VisitBracketedArgumentList(BracketedArgumentListSyntax node)
        {
            if (_visitBracketedArgumentList != null)
            {
                var visit = _visitBracketedArgumentList.Invoke(this, node);
                return (visit is BracketedArgumentListSyntax newNode) ? base.VisitBracketedArgumentList(newNode) : visit;
            }
            return base.VisitBracketedArgumentList(node);
        }

        /// <summary>
        /// Visits the specified argument syntax node.
        /// </summary>
        /// <param name="node">The argument syntax node to visit.</param>
        /// <returns>The visited argument syntax node, possibly modified.</returns>
        public override SyntaxNode VisitArgument(ArgumentSyntax node)
        {
            if (_visitArgument != null)
            {
                var visit = _visitArgument.Invoke(this, node);
                return (visit is ArgumentSyntax newNode) ? base.VisitArgument(newNode) : visit;
            }
            return base.VisitArgument(node);
        }

        /// <summary>
        /// Visits the specified expression colon syntax node.
        /// </summary>
        /// <param name="node">The expression colon syntax node to visit.</param>
        /// <returns>The visited expression colon syntax node, possibly modified.</returns>
        public override SyntaxNode VisitExpressionColon(ExpressionColonSyntax node)
        {
            if (_visitExpressionColon != null)
            {
                var visit = _visitExpressionColon.Invoke(this, node);
                return (visit is ExpressionColonSyntax newNode) ? base.VisitExpressionColon(newNode) : visit;
            }
            return base.VisitExpressionColon(node);
        }

        /// <summary>
        /// Visits the specified name colon syntax node.
        /// </summary>
        /// <param name="node">The name colon syntax node to visit.</param>
        /// <returns>The visited name colon syntax node, possibly modified.</returns>
        public override SyntaxNode VisitNameColon(NameColonSyntax node)
        {
            if (_visitNameColon != null)
            {
                var visit = _visitNameColon.Invoke(this, node);
                return (visit is NameColonSyntax newNode) ? base.VisitNameColon(newNode) : visit;
            }
            return base.VisitNameColon(node);
        }

        /// <summary>
        /// Visits the specified declaration expression syntax node.
        /// </summary>
        /// <param name="node">The declaration expression syntax node to visit.</param>
        /// <returns>The visited declaration expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitDeclarationExpression(DeclarationExpressionSyntax node)
        {
            if (_visitDeclarationExpression != null)
            {
                var visit = _visitDeclarationExpression.Invoke(this, node);
                return (visit is DeclarationExpressionSyntax newNode) ? base.VisitDeclarationExpression(newNode) : visit;
            }


            return base.VisitDeclarationExpression(node);
        }

        /// <summary>
        /// Visits the specified cast expression syntax node.
        /// </summary>
        /// <param name="node">The cast expression syntax node to visit.</param>
        /// <returns>The visited cast expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitCastExpression(CastExpressionSyntax node)
        {
            if (_visitCastExpression != null)
            {
                var visit = _visitCastExpression.Invoke(this, node);
                return (visit is CastExpressionSyntax newNode) ? base.VisitCastExpression(newNode) : visit;
            }
            return base.VisitCastExpression(node);
        }

        /// <summary>
        /// Visits the specified anonymous method expression syntax node.
        /// </summary>
        /// <param name="node">The anonymous method expression syntax node to visit.</param>
        /// <returns>The visited anonymous method expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
        {
            if (_visitAnonymousMethodExpression != null)
            {
                var visit = _visitAnonymousMethodExpression.Invoke(this, node);
                return (visit is AnonymousMethodExpressionSyntax newNode) ? base.VisitAnonymousMethodExpression(newNode) : visit;
            }
            return base.VisitAnonymousMethodExpression(node);
        }

        /// <summary>
        /// Visits the specified simple lambda expression syntax node.
        /// </summary>
        /// <param name="node">The simple lambda expression syntax node to visit.</param>
        /// <returns>The visited simple lambda expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
            if (_visitSimpleLambdaExpression != null)
            {
                var visit = _visitSimpleLambdaExpression.Invoke(this, node);
                return (visit is SimpleLambdaExpressionSyntax newNode) ? base.VisitSimpleLambdaExpression(newNode) : visit;
            }
            return base.VisitSimpleLambdaExpression(node);
        }

        /// <summary>
        /// Visits the specified ref expression syntax node.
        /// </summary>
        /// <param name="node">The ref expression syntax node to visit.</param>
        /// <returns>The visited ref expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitRefExpression(RefExpressionSyntax node)
        {
            if (_visitRefExpression != null)
            {
                var visit = _visitRefExpression.Invoke(this, node);
                return (visit is RefExpressionSyntax newNode) ? base.VisitRefExpression(newNode) : visit;
            }
            return base.VisitRefExpression(node);
        }

        /// <summary>
        /// Visits the specified parenthesized lambda expression syntax node.
        /// </summary>
        /// <param name="node">The parenthesized lambda expression syntax node to visit.</param>
        /// <returns>The visited parenthesized lambda expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
            if (_visitParenthesizedLambdaExpression != null)
            {
                var visit = _visitParenthesizedLambdaExpression.Invoke(this, node);
                return (visit is ParenthesizedLambdaExpressionSyntax newNode) ? base.VisitParenthesizedLambdaExpression(newNode) : visit;
            }
            return base.VisitParenthesizedLambdaExpression(node);
        }

        /// <summary>
        /// Visits the specified initializer expression syntax node.
        /// </summary>
        /// <param name="node">The initializer expression syntax node to visit.</param>
        /// <returns>The visited initializer expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitInitializerExpression(InitializerExpressionSyntax node)
        {
            if (_visitInitializerExpression != null)
            {
                var visit = _visitInitializerExpression.Invoke(this, node);
                return (visit is InitializerExpressionSyntax newNode) ? base.VisitInitializerExpression(newNode) : visit;
            }
            return base.VisitInitializerExpression(node);
        }

        /// <summary>
        /// Visits the specified implicit object creation expression syntax node.
        /// </summary>
        /// <param name="node">The implicit object creation expression syntax node to visit.</param>
        /// <returns>The visited implicit object creation expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitImplicitObjectCreationExpression(ImplicitObjectCreationExpressionSyntax node)
        {
            if (_visitImplicitObjectCreationExpression != null)
            {
                var visit = _visitImplicitObjectCreationExpression.Invoke(this, node);
                return (visit is ImplicitObjectCreationExpressionSyntax newNode) ? base.VisitImplicitObjectCreationExpression(newNode) : visit;
            }
            return base.VisitImplicitObjectCreationExpression(node);
        }

        /// <summary>
        /// Visits the specified object creation expression syntax node.
        /// </summary>
        /// <param name="node">The object creation expression syntax node to visit.</param>
        /// <returns>The visited object creation expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            if (_visitObjectCreationExpression != null)
            {
                var visit = _visitObjectCreationExpression.Invoke(this, node);
                return (visit is ObjectCreationExpressionSyntax newNode) ? base.VisitObjectCreationExpression(newNode) : visit;
            }
            return base.VisitObjectCreationExpression(node);
        }

        /// <summary>
        /// Visits the specified 'with' expression syntax node.
        /// </summary>
        /// <param name="node">The 'with' expression syntax node to visit.</param>
        /// <returns>The visited 'with' expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitWithExpression(WithExpressionSyntax node)
        {
            if (_visitWithExpression != null)
            {
                var visit = _visitWithExpression.Invoke(this, node);
                return (visit is WithExpressionSyntax newNode) ? base.VisitWithExpression(newNode) : visit;
            }
            return base.VisitWithExpression(node);
        }

        /// <summary>
        /// Visits the specified anonymous object member declarator syntax node.
        /// </summary>
        /// <param name="node">The anonymous object member declarator syntax node to visit.</param>
        /// <returns>The visited anonymous object member declarator syntax node, possibly modified.</returns>
        public override SyntaxNode VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node)
        {
            if (_visitAnonymousObjectMemberDeclarator != null)
            {
                var visit = _visitAnonymousObjectMemberDeclarator.Invoke(this, node);
                return (visit is AnonymousObjectMemberDeclaratorSyntax newNode) ? base.VisitAnonymousObjectMemberDeclarator(newNode) : visit;
            }
            return base.VisitAnonymousObjectMemberDeclarator(node);
        }

        /// <summary>
        /// Visits the specified anonymous object creation expression syntax node.
        /// </summary>
        /// <param name="node">The anonymous object creation expression syntax node to visit.</param>
        /// <returns>The visited anonymous object creation expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node)
        {
            if (_visitAnonymousObjectCreationExpression != null)
            {
                var visit = _visitAnonymousObjectCreationExpression.Invoke(this, node);
                return (visit is AnonymousObjectCreationExpressionSyntax newNode) ? base.VisitAnonymousObjectCreationExpression(newNode) : visit;
            }
            return base.VisitAnonymousObjectCreationExpression(node);
        }

        /// <summary>
        /// Visits the specified array creation expression syntax node.
        /// </summary>
        /// <param name="node">The array creation expression syntax node to visit.</param>
        /// <returns>The visited array creation expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
        {
            if (_visitArrayCreationExpression != null)
            {
                var visit = _visitArrayCreationExpression.Invoke(this, node);
                return (visit is ArrayCreationExpressionSyntax newNode) ? base.VisitArrayCreationExpression(newNode) : visit;
            }
            return base.VisitArrayCreationExpression(node);
        }

        /// <summary>
        /// Visits the specified implicit array creation expression syntax node.
        /// </summary>
        /// <param name="node">The implicit array creation expression syntax node to visit.</param>
        /// <returns>The visited implicit array creation expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
        {
            if (_visitImplicitArrayCreationExpression != null)
            {
                var visit = _visitImplicitArrayCreationExpression.Invoke(this, node);
                return (visit is ImplicitArrayCreationExpressionSyntax newNode) ? base.VisitImplicitArrayCreationExpression(newNode) : visit;
            }
            return base.VisitImplicitArrayCreationExpression(node);
        }

        /// <summary>
        /// Visits the specified stack alloc array creation expression syntax node.
        /// </summary>
        /// <param name="node">The stack alloc array creation expression syntax node to visit.</param>
        /// <returns>The visited stack alloc array creation expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitStackAllocArrayCreationExpression(StackAllocArrayCreationExpressionSyntax node)
        {
            if (_visitStackAllocArrayCreationExpression != null)
            {
                var visit = _visitStackAllocArrayCreationExpression.Invoke(this, node);
                return (visit is StackAllocArrayCreationExpressionSyntax newNode) ? base.VisitStackAllocArrayCreationExpression(newNode) : visit;
            }
            return base.VisitStackAllocArrayCreationExpression(node);
        }

        /// <summary>
        /// Visits the specified implicit stack alloc array creation expression syntax node.
        /// </summary>
        /// <param name="node">The implicit stack alloc array creation expression syntax node to visit.</param>
        /// <returns>The visited implicit stack alloc array creation expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitImplicitStackAllocArrayCreationExpression(ImplicitStackAllocArrayCreationExpressionSyntax node)
        {
            if (_visitImplicitStackAllocArrayCreationExpression != null)
            {
                var visit = _visitImplicitStackAllocArrayCreationExpression.Invoke(this, node);
                return (visit is ImplicitStackAllocArrayCreationExpressionSyntax newNode) ? base.VisitImplicitStackAllocArrayCreationExpression(newNode) : visit;
            }
            return base.VisitImplicitStackAllocArrayCreationExpression(node);
        }

        /// <summary>
        /// Visits the specified collection expression syntax node.
        /// </summary>
        /// <param name="node">The collection expression syntax node to visit.</param>
        /// <returns>The visited collection expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitCollectionExpression(CollectionExpressionSyntax node)
        {
            if (_visitCollectionExpression != null)
            {
                var visit = _visitCollectionExpression.Invoke(this, node);
                return (visit is CollectionExpressionSyntax newNode) ? base.VisitCollectionExpression(newNode) : visit;
            }
            return base.VisitCollectionExpression(node);
        }

        /// <summary>
        /// Visits the specified expression element syntax node.
        /// </summary>
        /// <param name="node">The expression element syntax node to visit.</param>
        /// <returns>The visited expression element syntax node, possibly modified.</returns>
        public override SyntaxNode VisitExpressionElement(ExpressionElementSyntax node)
        {
            if (_visitExpressionElement != null)
            {
                var visit = _visitExpressionElement.Invoke(this, node);
                return (visit is ExpressionElementSyntax newNode) ? base.VisitExpressionElement(newNode) : visit;
            }
            return base.VisitExpressionElement(node);
        }

        /// <summary>
        /// Visits the specified spread element syntax node.
        /// </summary>
        /// <param name="node">The spread element syntax node to visit.</param>
        /// <returns>The visited spread element syntax node, possibly modified.</returns>
        public override SyntaxNode VisitSpreadElement(SpreadElementSyntax node)
        {
            if (_visitSpreadElement != null)
            {
                var visit = _visitSpreadElement.Invoke(this, node);
                return (visit is SpreadElementSyntax newNode) ? base.VisitSpreadElement(newNode) : visit;
            }
            return base.VisitSpreadElement(node);
        }

        /// <summary>
        /// Visits the specified query expression syntax node.
        /// </summary>
        /// <param name="node">The query expression syntax node to visit.</param>
        /// <returns>The visited query expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitQueryExpression(QueryExpressionSyntax node)
        {
            if (_visitQueryExpression != null)
            {
                var visit = _visitQueryExpression.Invoke(this, node);


                return (visit is QueryExpressionSyntax newNode) ? base.VisitQueryExpression(newNode) : visit;
            }
            return base.VisitQueryExpression(node);
        }

        /// <summary>
        /// Visits the specified query body syntax node.
        /// </summary>
        /// <param name="node">The query body syntax node to visit.</param>
        /// <returns>The visited query body syntax node, possibly modified.</returns>
        public override SyntaxNode VisitQueryBody(QueryBodySyntax node)
        {
            if (_visitQueryBody != null)
            {
                var visit = _visitQueryBody.Invoke(this, node);
                return (visit is QueryBodySyntax newNode) ? base.VisitQueryBody(newNode) : visit;
            }
            return base.VisitQueryBody(node);
        }

        /// <summary>
        /// Visits the specified from clause syntax node.
        /// </summary>
        /// <param name="node">The from clause syntax node to visit.</param>
        /// <returns>The visited from clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitFromClause(FromClauseSyntax node)
        {
            if (_visitFromClause != null)
            {
                var visit = _visitFromClause.Invoke(this, node);
                return (visit is FromClauseSyntax newNode) ? base.VisitFromClause(newNode) : visit;
            }
            return base.VisitFromClause(node);
        }

        /// <summary>
        /// Visits the specified let clause syntax node.
        /// </summary>
        /// <param name="node">The let clause syntax node to visit.</param>
        /// <returns>The visited let clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitLetClause(LetClauseSyntax node)
        {
            if (_visitLetClause != null)
            {
                var visit = _visitLetClause.Invoke(this, node);
                return (visit is LetClauseSyntax newNode) ? base.VisitLetClause(newNode) : visit;
            }
            return base.VisitLetClause(node);
        }

        /// <summary>
        /// Visits the specified join clause syntax node.
        /// </summary>
        /// <param name="node">The join clause syntax node to visit.</param>
        /// <returns>The visited join clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitJoinClause(JoinClauseSyntax node)
        {
            if (_visitJoinClause != null)
            {
                var visit = _visitJoinClause.Invoke(this, node);
                return (visit is JoinClauseSyntax newNode) ? base.VisitJoinClause(newNode) : visit;
            }
            return base.VisitJoinClause(node);
        }

        /// <summary>
        /// Visits the specified join into clause syntax node.
        /// </summary>
        /// <param name="node">The join into clause syntax node to visit.</param>
        /// <returns>The visited join into clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitJoinIntoClause(JoinIntoClauseSyntax node)
        {
            if (_visitJoinIntoClause != null)
            {
                var visit = _visitJoinIntoClause.Invoke(this, node);
                return (visit is JoinIntoClauseSyntax newNode) ? base.VisitJoinIntoClause(newNode) : visit;
            }
            return base.VisitJoinIntoClause(node);
        }

        /// <summary>
        /// Visits the specified where clause syntax node.
        /// </summary>
        /// <param name="node">The where clause syntax node to visit.</param>
        /// <returns>The visited where clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitWhereClause(WhereClauseSyntax node)
        {
            if (_visitWhereClause != null)
            {
                var visit = _visitWhereClause.Invoke(this, node);
                return (visit is WhereClauseSyntax newNode) ? base.VisitWhereClause(newNode) : visit;
            }
            return base.VisitWhereClause(node);
        }

        /// <summary>
        /// Visits the specified order by clause syntax node.
        /// </summary>
        /// <param name="node">The order by clause syntax node to visit.</param>
        /// <returns>The visited order by clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitOrderByClause(OrderByClauseSyntax node)
        {
            if (_visitOrderByClause != null)
            {
                var visit = _visitOrderByClause.Invoke(this, node);
                return (visit is OrderByClauseSyntax newNode) ? base.VisitOrderByClause(newNode) : visit;
            }
            return base.VisitOrderByClause(node);
        }

        /// <summary>
        /// Visits the specified ordering syntax node.
        /// </summary>
        /// <param name="node">The ordering syntax node to visit.</param>
        /// <returns>The visited ordering syntax node, possibly modified.</returns>
        public override SyntaxNode VisitOrdering(OrderingSyntax node)
        {
            if (_visitOrdering != null)
            {
                var visit = _visitOrdering.Invoke(this, node);
                return (visit is OrderingSyntax newNode) ? base.VisitOrdering(newNode) : visit;
            }
            return base.VisitOrdering(node);
        }

        /// <summary>
        /// Visits the specified select clause syntax node.
        /// </summary>
        /// <param name="node">The select clause syntax node to visit.</param>
        /// <returns>The visited select clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitSelectClause(SelectClauseSyntax node)
        {
            if (_visitSelectClause != null)
            {
                var visit = _visitSelectClause.Invoke(this, node);
                return (visit is SelectClauseSyntax newNode) ? base.VisitSelectClause(newNode) : visit;
            }
            return base.VisitSelectClause(node);
        }

        /// <summary>
        /// Visits the specified group clause syntax node.
        /// </summary>
        /// <param name="node">The group clause syntax node to visit.</param>
        /// <returns>The visited group clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitGroupClause(GroupClauseSyntax node)
        {
            if (_visitGroupClause != null)
            {
                var visit = _visitGroupClause.Invoke(this, node);
                return (visit is GroupClauseSyntax newNode) ? base.VisitGroupClause(newNode) : visit;
            }
            return base.VisitGroupClause(node);
        }

        /// <summary>
        /// Visits the specified query continuation syntax node.
        /// </summary>
        /// <param name="node">The query continuation syntax node to visit.</param>
        /// <returns>The visited query continuation syntax node, possibly modified.</returns>
        public override SyntaxNode VisitQueryContinuation(QueryContinuationSyntax node)
        {
            if (_visitQueryContinuation != null)
            {
                var visit = _visitQueryContinuation.Invoke(this, node);
                return (visit is QueryContinuationSyntax newNode) ? base.VisitQueryContinuation(newNode) : visit;
            }
            return base.VisitQueryContinuation(node);
        }

        /// <summary>
        /// Visits the specified omitted array size expression syntax node.
        /// </summary>
        /// <param name="node">The omitted array size expression syntax node to visit.</param>
        /// <returns>The visited omitted array size expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax node)
        {
            if (_visitOmittedArraySizeExpression != null)
            {
                var visit = _visitOmittedArraySizeExpression.Invoke(this, node);
                return (visit is OmittedArraySizeExpressionSyntax newNode) ? base.VisitOmittedArraySizeExpression(newNode) : visit;
            }
            return base.VisitOmittedArraySizeExpression(node);
        }

        /// <summary>
        /// Visits the specified interpolated string expression syntax node.
        /// </summary>
        /// <param name="node">The interpolated string expression syntax node to visit.</param>
        /// <returns>The visited interpolated string expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
        {
            if (_visitInterpolatedStringExpression != null)
            {
                var visit = _visitInterpolatedStringExpression.Invoke(this, node);
                return (visit is InterpolatedStringExpressionSyntax newNode) ? base.VisitInterpolatedStringExpression(newNode) : visit;
            }
            return base.VisitInterpolatedStringExpression(node);
        }

        /// <summary>
        /// Visits the specified 'is pattern' expression syntax node.
        /// </summary>
        /// <param name="node">The 'is pattern' expression syntax node to visit.</param>
        /// <returns>The visited 'is pattern' expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitIsPatternExpression(IsPatternExpressionSyntax node)
        {
            if (_visitIsPatternExpression != null)
            {
                var visit = _visitIsPatternExpression.Invoke(this, node);
                return (visit is IsPatternExpressionSyntax newNode) ? base.VisitIsPatternExpression(newNode) : visit;
            }
            return base.VisitIsPatternExpression(node);
        }

        /// <summary>
        /// Visits the specified throw expression syntax node.
        /// </summary>
        /// <param name="node">The throw expression syntax node to visit.</param>
        /// <returns>The visited throw expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitThrowExpression(ThrowExpressionSyntax node)
        {
            if (_visitThrowExpression != null)
            {
                var visit = _visitThrowExpression.Invoke(this, node);
                return (visit is ThrowExpressionSyntax newNode) ? base.VisitThrowExpression(newNode) : visit;
            }
            return base.VisitThrowExpression(node);
        }

        /// <summary>
        /// Visits the specified when clause syntax node.
        /// </summary>
        /// <param name="node">The when clause syntax node to visit.</param>
        /// <returns>The visited when clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitWhenClause(WhenClauseSyntax node)
        {
            if (_visitWhenClause != null)
            {
                var visit = _visitWhenClause.Invoke(this, node);
                return (visit is WhenClauseSyntax newNode) ? base.VisitWhenClause(newNode) : visit;
            }
            return base.VisitWhenClause(node);
        }

        /// <summary>
        /// Visits the specified discard pattern syntax node.
        /// </summary>
        /// <param name="node">The discard pattern syntax node to visit.</param>
        /// <returns>The visited discard pattern syntax node, possibly modified.</returns>
        public override SyntaxNode VisitDiscardPattern(DiscardPatternSyntax node)
        {
            if (_visitDiscardPattern != null)
            {
                var visit = _visitDiscardPattern.Invoke(this, node);
                return (visit is DiscardPatternSyntax newNode) ? base.VisitDiscardPattern(newNode) : visit;
            }
            return base.VisitDiscardPattern(node);
        }

        /// <summary>
        /// Visits the specified declaration pattern syntax node.
        /// </summary>
        /// <param name="node">The declaration pattern syntax node to visit.</param>
        /// <returns>The visited declaration pattern syntax node, possibly modified.</returns>
        public override SyntaxNode VisitDeclarationPattern(DeclarationPatternSyntax node)
        {
            if (_visitDeclarationPattern != null)
            {
                var visit = _visitDeclarationPattern.Invoke(this, node);
                return (visit is DeclarationPatternSyntax newNode) ? base.VisitDeclarationPattern(newNode) : visit;
            }
            return base.VisitDeclarationPattern(node);
        }

        /// <summary>
        /// Visits the specified var pattern syntax node.
        /// </summary>
        /// <param name="node">The var pattern syntax node to visit.</param>
        /// <returns>The visited var pattern syntax node, possibly modified.</returns>
        public override SyntaxNode VisitVarPattern(VarPatternSyntax node)
        {
            if (_visitVarPattern != null)
            {
                var visit = _visitVarPattern.Invoke(this, node);
                return (visit is VarPatternSyntax newNode) ? base.VisitVarPattern(newNode) : visit;
            }
            return base.VisitVarPattern(node);
        }

        /// <summary>
        /// Visits the specified recursive pattern syntax node.
        /// </summary>
        /// <param name="node">The recursive pattern syntax node to visit.</param>
        /// <returns>The visited recursive pattern syntax node, possibly modified.</returns>
        public override SyntaxNode VisitRecursivePattern(RecursivePatternSyntax node)
        {
            if (_visitRecursivePattern != null)
            {
                var visit = _visitRecursivePattern.Invoke(this, node);
                return (visit is RecursivePatternSyntax newNode) ? base.VisitRecursivePattern(newNode) : visit;
            }
            return base.VisitRecursivePattern(node);
        }

        /// <summary>
        /// Visits the specified positional pattern clause syntax node.
        /// </summary>
        /// <param name="node">The positional pattern clause syntax node to visit.</param>
        /// <returns>The visited positional pattern clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitPositionalPatternClause(PositionalPatternClauseSyntax node)
        {
            if (_visitPositionalPatternClause != null)
            {
                var visit = _visitPositionalPatternClause.Invoke(this, node);
                return (visit is PositionalPatternClauseSyntax newNode) ? base.VisitPositionalPatternClause(newNode) : visit;
            }
            return base.VisitPositionalPatternClause(node);
        }

        /// <summary>
        /// Visits the specified property pattern clause syntax node.
        /// </summary>
        /// <param name="node">The property pattern clause syntax node to visit.</param>
        /// <returns>The visited property pattern clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitPropertyPatternClause(PropertyPatternClauseSyntax node)
        {
            if (_visitPropertyPatternClause != null)
            {
                var visit = _visitPropertyPatternClause.Invoke(this, node);
                return (visit is PropertyPatternClauseSyntax newNode) ? base.VisitPropertyPatternClause(newNode) : visit;
            }
            return base.VisitPropertyPatternClause(node);
        }

        /// <summary>
        /// Visits the specified subpattern syntax node.
        /// </summary>
        /// <param name="node">The subpattern syntax node to visit.</param>
        /// <returns>The visited subpattern syntax node, possibly modified.</returns>
        public override SyntaxNode VisitSubpattern(SubpatternSyntax node)
        {
            if (_visitSubpattern != null)
            {
                var visit = _visitSubpattern.Invoke(this, node);
                return (visit is SubpatternSyntax newNode) ? base.VisitSubpattern(newNode) : visit;


            }
            return base.VisitSubpattern(node);
        }

        /// <summary>
        /// Visits the specified constant pattern syntax node.
        /// </summary>
        /// <param name="node">The constant pattern syntax node to visit.</param>
        /// <returns>The visited constant pattern syntax node, possibly modified.</returns>
        public override SyntaxNode VisitConstantPattern(ConstantPatternSyntax node)
        {
            if (_visitConstantPattern != null)
            {
                var visit = _visitConstantPattern.Invoke(this, node);
                return (visit is ConstantPatternSyntax newNode) ? base.VisitConstantPattern(newNode) : visit;
            }
            return base.VisitConstantPattern(node);
        }

        /// <summary>
        /// Visits the specified parenthesized pattern syntax node.
        /// </summary>
        /// <param name="node">The parenthesized pattern syntax node to visit.</param>
        /// <returns>The visited parenthesized pattern syntax node, possibly modified.</returns>
        public override SyntaxNode VisitParenthesizedPattern(ParenthesizedPatternSyntax node)
        {
            if (_visitParenthesizedPattern != null)
            {
                var visit = _visitParenthesizedPattern.Invoke(this, node);
                return (visit is ParenthesizedPatternSyntax newNode) ? base.VisitParenthesizedPattern(newNode) : visit;
            }
            return base.VisitParenthesizedPattern(node);
        }

        /// <summary>
        /// Visits the specified relational pattern syntax node.
        /// </summary>
        /// <param name="node">The relational pattern syntax node to visit.</param>
        /// <returns>The visited relational pattern syntax node, possibly modified.</returns>
        public override SyntaxNode VisitRelationalPattern(RelationalPatternSyntax node)
        {
            if (_visitRelationalPattern != null)
            {
                var visit = _visitRelationalPattern.Invoke(this, node);
                return (visit is RelationalPatternSyntax newNode) ? base.VisitRelationalPattern(newNode) : visit;
            }
            return base.VisitRelationalPattern(node);
        }

        /// <summary>
        /// Visits the specified type pattern syntax node.
        /// </summary>
        /// <param name="node">The type pattern syntax node to visit.</param>
        /// <returns>The visited type pattern syntax node, possibly modified.</returns>
        public override SyntaxNode VisitTypePattern(TypePatternSyntax node)
        {
            if (_visitTypePattern != null)
            {
                var visit = _visitTypePattern.Invoke(this, node);
                return (visit is TypePatternSyntax newNode) ? base.VisitTypePattern(newNode) : visit;
            }
            return base.VisitTypePattern(node);
        }

        /// <summary>
        /// Visits the specified binary pattern syntax node.
        /// </summary>
        /// <param name="node">The binary pattern syntax node to visit.</param>
        /// <returns>The visited binary pattern syntax node, possibly modified.</returns>
        public override SyntaxNode VisitBinaryPattern(BinaryPatternSyntax node)
        {
            if (_visitBinaryPattern != null)
            {
                var visit = _visitBinaryPattern.Invoke(this, node);
                return (visit is BinaryPatternSyntax newNode) ? base.VisitBinaryPattern(newNode) : visit;
            }
            return base.VisitBinaryPattern(node);
        }

        /// <summary>
        /// Visits the specified unary pattern syntax node.
        /// </summary>
        /// <param name="node">The unary pattern syntax node to visit.</param>
        /// <returns>The visited unary pattern syntax node, possibly modified.</returns>
        public override SyntaxNode VisitUnaryPattern(UnaryPatternSyntax node)
        {
            if (_visitUnaryPattern != null)
            {
                var visit = _visitUnaryPattern.Invoke(this, node);
                return (visit is UnaryPatternSyntax newNode) ? base.VisitUnaryPattern(newNode) : visit;
            }
            return base.VisitUnaryPattern(node);
        }

        /// <summary>
        /// Visits the specified list pattern syntax node.
        /// </summary>
        /// <param name="node">The list pattern syntax node to visit.</param>
        /// <returns>The visited list pattern syntax node, possibly modified.</returns>
        public override SyntaxNode VisitListPattern(ListPatternSyntax node)
        {
            if (_visitListPattern != null)
            {
                var visit = _visitListPattern.Invoke(this, node);
                return (visit is ListPatternSyntax newNode) ? base.VisitListPattern(newNode) : visit;
            }
            return base.VisitListPattern(node);
        }

        /// <summary>
        /// Visits the specified slice pattern syntax node.
        /// </summary>
        /// <param name="node">The slice pattern syntax node to visit.</param>
        /// <returns>The visited slice pattern syntax node, possibly modified.</returns>
        public override SyntaxNode VisitSlicePattern(SlicePatternSyntax node)
        {
            if (_visitSlicePattern != null)
            {
                var visit = _visitSlicePattern.Invoke(this, node);
                return (visit is SlicePatternSyntax newNode) ? base.VisitSlicePattern(newNode) : visit;
            }
            return base.VisitSlicePattern(node);
        }

        /// <summary>
        /// Visits the specified interpolated string text syntax node.
        /// </summary>
        /// <param name="node">The interpolated string text syntax node to visit.</param>
        /// <returns>The visited interpolated string text syntax node, possibly modified.</returns>
        public override SyntaxNode VisitInterpolatedStringText(InterpolatedStringTextSyntax node)
        {
            if (_visitInterpolatedStringText != null)
            {
                var visit = _visitInterpolatedStringText.Invoke(this, node);
                return (visit is InterpolatedStringTextSyntax newNode) ? base.VisitInterpolatedStringText(newNode) : visit;
            }
            return base.VisitInterpolatedStringText(node);
        }

        /// <summary>
        /// Visits the specified interpolation syntax node.
        /// </summary>
        /// <param name="node">The interpolation syntax node to visit.</param>
        /// <returns>The visited interpolation syntax node, possibly modified.</returns>
        public override SyntaxNode VisitInterpolation(InterpolationSyntax node)
        {
            if (_visitInterpolation != null)
            {
                var visit = _visitInterpolation.Invoke(this, node);
                return (visit is InterpolationSyntax newNode) ? base.VisitInterpolation(newNode) : visit;
            }
            return base.VisitInterpolation(node);
        }

        /// <summary>
        /// Visits the specified interpolation alignment clause syntax node.
        /// </summary>
        /// <param name="node">The interpolation alignment clause syntax node to visit.</param>
        /// <returns>The visited interpolation alignment clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitInterpolationAlignmentClause(InterpolationAlignmentClauseSyntax node)
        {
            if (_visitInterpolationAlignmentClause != null)
            {
                var visit = _visitInterpolationAlignmentClause.Invoke(this, node);
                return (visit is InterpolationAlignmentClauseSyntax newNode) ? base.VisitInterpolationAlignmentClause(newNode) : visit;
            }
            return base.VisitInterpolationAlignmentClause(node);
        }

        /// <summary>
        /// Visits the specified interpolation format clause syntax node.
        /// </summary>
        /// <param name="node">The interpolation format clause syntax node to visit.</param>
        /// <returns>The visited interpolation format clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitInterpolationFormatClause(InterpolationFormatClauseSyntax node)
        {
            if (_visitInterpolationFormatClause != null)
            {
                var visit = _visitInterpolationFormatClause.Invoke(this, node);
                return (visit is InterpolationFormatClauseSyntax newNode) ? base.VisitInterpolationFormatClause(newNode) : visit;
            }
            return base.VisitInterpolationFormatClause(node);
        }

        /// <summary>
        /// Visits the specified global statement syntax node.
        /// </summary>
        /// <param name="node">The global statement syntax node to visit.</param>
        /// <returns>The visited global statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitGlobalStatement(GlobalStatementSyntax node)
        {
            if (_visitGlobalStatement != null)
            {
                var visit = _visitGlobalStatement.Invoke(this, node);
                return (visit is GlobalStatementSyntax newNode) ? base.VisitGlobalStatement(newNode) : visit;
            }
            return base.VisitGlobalStatement(node);
        }

        /// <summary>
        /// Visits the specified block syntax node.
        /// </summary>
        /// <param name="node">The block syntax node to visit.</param>
        /// <returns>The visited block syntax node, possibly modified.</returns>
        public override SyntaxNode VisitBlock(BlockSyntax node)
        {
            if (_visitBlock != null)
            {
                var visit = _visitBlock.Invoke(this, node);
                return (visit is BlockSyntax newNode) ? base.VisitBlock(newNode) : visit;
            }
            return base.VisitBlock(node);
        }

        /// <summary>
        /// Visits the specified local function statement syntax node.
        /// </summary>
        /// <param name="node">The local function statement syntax node to visit.</param>
        /// <returns>The visited local function statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
        {
            if (_visitLocalFunctionStatement != null)
            {
                var visit = _visitLocalFunctionStatement.Invoke(this, node);
                return (visit is LocalFunctionStatementSyntax newNode) ? base.VisitLocalFunctionStatement(newNode) : visit;
            }
            return base.VisitLocalFunctionStatement(node);
        }

        /// <summary>
        /// Visits the specified local declaration statement syntax node.
        /// </summary>
        /// <param name="node">The local declaration statement syntax node to visit.</param>
        /// <returns>The visited local declaration statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            if (_visitLocalDeclarationStatement != null)
            {
                var visit = _visitLocalDeclarationStatement.Invoke(this, node);
                return (visit is LocalDeclarationStatementSyntax newNode) ? base.VisitLocalDeclarationStatement(newNode) : visit;
            }
            return base.VisitLocalDeclarationStatement(node);
        }

        /// <summary>
        /// Visits the specified variable declaration syntax node.
        /// </summary>
        /// <param name="node">The variable declaration syntax node to visit.</param>
        /// <returns>The visited variable declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            if (_visitVariableDeclaration != null)
            {
                var visit = _visitVariableDeclaration.Invoke(this, node);
                return (visit is VariableDeclarationSyntax newNode) ? base.VisitVariableDeclaration(newNode) : visit;
            }
            return base.VisitVariableDeclaration(node);
        }

        /// <summary>
        /// Visits the specified variable declarator syntax node.
        /// </summary>
        /// <param name="node">The variable declarator syntax node to visit.</param>
        /// <returns>The visited variable declarator syntax node, possibly modified.</returns>
        public override SyntaxNode VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            if (_visitVariableDeclarator != null)
            {
                var visit = _visitVariableDeclarator.Invoke(this, node);
                return (visit is VariableDeclaratorSyntax newNode) ? base.VisitVariableDeclarator(newNode) : visit;
            }
            return base.VisitVariableDeclarator(node);
        }

        /// <summary>
        /// Visits the specified equals value clause syntax node.
        /// </summary>
        /// <param name="node">The equals value clause syntax node to visit.</param>
        /// <returns>The visited equals value clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitEqualsValueClause(EqualsValueClauseSyntax node)
        {
            if (_visitEqualsValueClause != null)
            {
                var visit = _visitEqualsValueClause.Invoke(this, node);
                return (visit is EqualsValueClauseSyntax newNode) ? base.VisitEqualsValueClause(newNode) : visit;
            }
            return base.VisitEqualsValueClause(node);
        }

        /// <summary>
        /// Visits the specified single variable designation syntax node.
        /// </summary>
        /// <param name="node">The single variable designation syntax node to visit.</param>
        /// <returns>The visited single variable designation syntax node, possibly modified.</returns>
        public override SyntaxNode VisitSingleVariableDesignation(SingleVariableDesignationSyntax node)
        {
            if (_visitSingleVariableDesignation != null)
            {
                var visit = _visitSingleVariableDesignation.Invoke(this, node);
                return (visit is SingleVariableDesignationSyntax newNode) ? base.VisitSingleVariableDesignation(newNode) : visit;
            }
            return base.VisitSingleVariableDesignation(node);
        }

        /// <summary>
        /// Visits the specified discard designation syntax node.
        /// </summary>
        /// <param name="node">The discard designation syntax node to visit.</param>
        /// <returns>The visited discard designation syntax node, possibly modified.</returns>
        public override SyntaxNode VisitDiscardDesignation(DiscardDesignationSyntax node)
        {
            if (_visitDiscardDesignation != null)
            {
                var visit = _visitDiscardDesignation.Invoke(this, node);
                return (visit is DiscardDesignationSyntax newNode) ? base.VisitDiscardDesignation(newNode) : visit;
            }
            return base.VisitDiscardDesignation(node);
        }

        /// <summary>
        /// Visits the specified parenthesized variable designation syntax node.
        /// </summary>
        /// <param name="node">The parenthesized variable designation syntax node to visit.</param>
        /// <returns>The visited parenthesized variable designation syntax node, possibly modified.</returns>
        public override SyntaxNode VisitParenthesizedVariableDesignation(ParenthesizedVariableDesignationSyntax node)
        {
            if (_visitParenthesizedVariableDesignation != null)
            {
                var visit = _visitParenthesizedVariableDesignation.Invoke(this, node);
                return (visit is ParenthesizedVariableDesignationSyntax newNode) ? base.VisitParenthesizedVariableDesignation(newNode) : visit;
            }
            return base.VisitParenthesizedVariableDesignation(node);
        }

        /// <summary>
        /// Visits the specified expression statement syntax node.
        /// </summary>
        /// <param name="node">The expression statement syntax node to visit.</param>
        /// <returns>The visited expression statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            if (_visitExpressionStatement != null)
            {
                var visit = _visitExpressionStatement.Invoke(this, node);
                return (visit is ExpressionStatementSyntax newNode) ? base.VisitExpressionStatement(newNode) : visit;
            }
            return base.VisitExpressionStatement(node);
        }

        /// <summary>
        /// Visits the specified empty statement syntax node.
        /// </summary>
        /// <param name="node">The empty statement syntax node to visit.</param>
        /// <returns>The visited empty statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitEmptyStatement(EmptyStatementSyntax node)
        {
            if (_visitEmptyStatement != null)
            {
                var visit = _visitEmptyStatement.Invoke(this, node);
                return (visit is EmptyStatementSyntax newNode) ? base.VisitEmptyStatement(newNode) : visit;
            }
            return base.VisitEmptyStatement(node);
        }

        /// <summary>
        /// Visits the specified labeled statement syntax node.
        /// </summary>
        /// <param name="node">The labeled statement syntax node to visit.</param>
        /// <returns>The visited labeled statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitLabeledStatement(LabeledStatementSyntax node)
        {
            if (_visitLabeledStatement != null)
            {
                var visit = _visitLabeledStatement.Invoke(this, node);
                return (visit is LabeledStatementSyntax newNode) ? base.VisitLabeledStatement(newNode) : visit;
            }
            return base.VisitLabeledStatement(node);
        }

        /// <summary>
        /// Visits the specified goto statement syntax node.
        /// </summary>
        /// <param name="node">The goto statement syntax node to visit.</param>
        /// <returns>The visited goto statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitGotoStatement(GotoStatementSyntax node)
        {
            if (_visitGotoStatement != null)
            {
                var visit = _visitGotoStatement.Invoke(this, node);
                return (visit is GotoStatementSyntax newNode) ? base.VisitGotoStatement(newNode) : visit;
            }
            return base.VisitGotoStatement(node);
        }

        /// <summary>
        /// Visits the specified break statement syntax node.
        /// </summary>
        /// <param name="node">The break statement syntax node to visit.</param>
        /// <returns>The visited break statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitBreakStatement(BreakStatementSyntax node)
        {
            if (_visitBreakStatement != null)
            {
                var visit = _visitBreakStatement.Invoke(this, node);
                return (visit is BreakStatementSyntax newNode) ? base.VisitBreakStatement(newNode) : visit;
            }
            return base.VisitBreakStatement(node);
        }

        /// <summary>
        /// Visits the specified continue statement syntax node.
        /// </summary>
        /// <param name="node">The continue statement syntax node to visit.</param>
        /// <returns>The visited continue statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitContinueStatement(ContinueStatementSyntax node)
        {
            if (_visitContinueStatement != null)
            {
                var visit = _visitContinueStatement.Invoke(this, node);
                return (visit is ContinueStatementSyntax newNode) ? base.VisitContinueStatement(newNode) : visit;
            }
            return base.VisitContinueStatement(node);
        }

        /// <summary>
        /// Visits the specified return statement syntax node.
        /// </summary>
        /// <param name="node">The return statement syntax node to visit.</param>
        /// <returns>The visited return statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitReturnStatement(ReturnStatementSyntax node)
        {
            if (_visitReturnStatement != null)
            {
                var visit = _visitReturnStatement.Invoke(this, node);
                return (visit is ReturnStatementSyntax newNode) ? base.VisitReturnStatement(newNode) : visit;
            }
            return base.VisitReturnStatement(node);
        }

        /// <summary>
        /// Visits the specified throw statement syntax node.
        /// </summary>
        /// <param name="node">The throw statement syntax node to visit.</param>
        /// <returns>The visited throw statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitThrowStatement(ThrowStatementSyntax node)
        {
            if (_visitThrowStatement != null)
            {
                var visit = _visitThrowStatement.Invoke(this, node);
                return (visit is ThrowStatementSyntax newNode) ? base.VisitThrowStatement(newNode) : visit;
            }
            return base.VisitThrowStatement(node);
        }

        /// <summary>
        /// Visits the specified yield statement syntax node.
        /// </summary>
        /// <param name="node">The yield statement syntax node to visit.</param>
        /// <returns>The visited yield statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitYieldStatement(YieldStatementSyntax node)
        {
            if (_visitYieldStatement != null)
            {
                var visit = _visitYieldStatement.Invoke(this, node);
                return (visit is YieldStatementSyntax newNode) ? base.VisitYieldStatement(newNode) : visit;
            }
            return base.VisitYieldStatement(node);
        }

        /// <summary>
        /// Visits the specified while statement syntax node.
        /// </summary>
        /// <param name="node">The while statement syntax node to visit.</param>
        /// <returns>The visited while statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitWhileStatement(WhileStatementSyntax node)
        {
            if (_visitWhileStatement != null)
            {
                var visit = _visitWhileStatement.Invoke(this, node);
                return (visit is WhileStatementSyntax newNode) ? base.VisitWhileStatement(newNode) : visit;
            }
            return base.VisitWhileStatement(node);
        }

        /// <summary>
        /// Visits the specified do statement syntax node.
        /// </summary>
        /// <param name="node">The do statement syntax node to visit.</param>
        /// <returns>The visited do statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitDoStatement(DoStatementSyntax node)
        {
            if (_visitDoStatement != null)
            {
                var visit = _visitDoStatement.Invoke(this, node);
                return (visit is DoStatementSyntax newNode) ? base.VisitDoStatement(newNode) : visit;
            }
            return base.VisitDoStatement(node);
        }

        /// <summary>
        /// Visits the specified for statement syntax node.
        /// </summary>
        /// <param name="node">The for statement syntax node to visit.</param>
        /// <returns>The visited for statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitForStatement(ForStatementSyntax node)
        {
            if (_visitForStatement != null)
            {
                var visit = _visitForStatement.Invoke(this, node);
                return (visit is ForStatementSyntax newNode) ? base.VisitForStatement(newNode) : visit;
            }
            return base.VisitForStatement(node);
        }

        /// <summary>
        /// Visits the specified foreach statement syntax node.
        /// </summary>
        /// <param name="node">The foreach statement syntax node to visit.</param>
        /// <returns>The visited foreach statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitForEachStatement(ForEachStatementSyntax node)
        {
            if (_visitForEachStatement != null)
            {
                var visit = _visitForEachStatement.Invoke(this, node);
                return (visit is ForEachStatementSyntax newNode) ? base.VisitForEachStatement(newNode) : visit;
            }
            return base.VisitForEachStatement(node);
        }

        /// <summary>
        /// Visits the specified foreach variable statement syntax node.
        /// </summary>
        /// <param name="node">The foreach variable statement syntax node to visit.</param>
        /// <returns>The visited foreach variable statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitForEachVariableStatement(ForEachVariableStatementSyntax node)
        {
            if (_visitForEachVariableStatement != null)
            {
                var visit = _visitForEachVariableStatement.Invoke(this, node);
                return (visit is ForEachVariableStatementSyntax newNode) ? base.VisitForEachVariableStatement(newNode) : visit;
            }
            return base.VisitForEachVariableStatement(node);
        }

        /// <summary>
        /// Visits the specified using statement syntax node.
        /// </summary>
        /// <param name="node">The using statement syntax node to visit.</param>
        /// <returns>The visited using statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitUsingStatement(UsingStatementSyntax node)
        {
            if (_visitUsingStatement != null)
            {
                var visit = _visitUsingStatement.Invoke(this, node);
                return (visit is UsingStatementSyntax newNode) ? base.VisitUsingStatement(newNode) : visit;
            }
            return base.VisitUsingStatement(node);
        }

        /// <summary>
        /// Visits the specified fixed statement syntax node.
        /// </summary>
        /// <param name="node">The fixed statement syntax node to visit.</param>
        /// <returns>The visited fixed statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitFixedStatement(FixedStatementSyntax node)
        {
            if (_visitFixedStatement != null)
            {
                var visit = _visitFixedStatement.Invoke(this, node);
                return (visit is FixedStatementSyntax newNode) ? base.VisitFixedStatement(newNode) : visit;
            }
            return base.VisitFixedStatement(node);
        }

        /// <summary>
        /// Visits the specified checked statement syntax node.
        /// </summary>
        /// <param name="node">The checked statement syntax node to visit.</param>
        /// <returns>The visited checked statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitCheckedStatement(CheckedStatementSyntax node)
        {
            if (_visitCheckedStatement != null)
            {
                var visit = _visitCheckedStatement.Invoke(this, node);
                return (visit is CheckedStatementSyntax newNode) ? base.VisitCheckedStatement(newNode) : visit;
            }
            return base.VisitCheckedStatement(node);
        }

        /// <summary>
        /// Visits the specified unsafe statement syntax node.
        /// </summary>
        /// <param name="node">The unsafe statement syntax node to visit.</param>
        /// <returns>The visited unsafe statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitUnsafeStatement(UnsafeStatementSyntax node)
        {
            if (_visitUnsafeStatement != null)
            {
                var visit = _visitUnsafeStatement.Invoke(this, node);
                return (visit is UnsafeStatementSyntax newNode) ? base.VisitUnsafeStatement(newNode) : visit;
            }
            return base.VisitUnsafeStatement(node);
        }

        /// <summary>
        /// Visits the specified lock statement syntax node.
        /// </summary>
        /// <param name="node">The lock statement syntax node to visit.</param>
        /// <returns>The visited lock statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitLockStatement(LockStatementSyntax node)
        {
            if (_visitLockStatement != null)
            {
                var visit = _visitLockStatement.Invoke(this, node);
                return (visit is LockStatementSyntax newNode) ? base.VisitLockStatement(newNode) : visit;
            }
            return base.VisitLockStatement(node);
        }

        /// <summary>
        /// Visits the specified if statement syntax node.
        /// </summary>
        /// <param name="node">The if statement syntax node to visit.</param>
        /// <returns>The visited if statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitIfStatement(IfStatementSyntax node)
        {
            if (_visitIfStatement != null)
            {
                var visit = _visitIfStatement.Invoke(this, node);
                return (visit is IfStatementSyntax newNode) ? base.VisitIfStatement(newNode) : visit;
            }
            return base.VisitIfStatement(node);
        }

        /// <summary>
        /// Visits the specified else clause syntax node.
        /// </summary>
        /// <param name="node">The else clause syntax node to visit.</param>
        /// <returns>The visited else clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitElseClause(ElseClauseSyntax node)
        {
            if (_visitElseClause != null)
            {
                var visit = _visitElseClause.Invoke(this, node);
                return (visit is ElseClauseSyntax newNode) ? base.VisitElseClause(newNode) : visit;
            }
            return base.VisitElseClause(node);
        }

        /// <summary>
        /// Visits the specified switch statement syntax node.
        /// </summary>
        /// <param name="node">The switch statement syntax node to visit.</param>
        /// <returns>The visited switch statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitSwitchStatement(SwitchStatementSyntax node)
        {
            if (_visitSwitchStatement != null)
            {
                var visit = _visitSwitchStatement.Invoke(this, node);
                return (visit is SwitchStatementSyntax newNode) ? base.VisitSwitchStatement(newNode) : visit;
            }
            return base.VisitSwitchStatement(node);
        }

        /// <summary>
        /// Visits the specified switch section syntax node.
        /// </summary>
        /// <param name="node">The switch section syntax node to visit.</param>
        /// <returns>The visited switch section syntax node, possibly modified.</returns>
        public override SyntaxNode VisitSwitchSection(SwitchSectionSyntax node)
        {
            if (_visitSwitchSection != null)
            {
                var visit = _visitSwitchSection.Invoke(this, node);
                return (visit is SwitchSectionSyntax newNode) ? base.VisitSwitchSection(newNode) : visit;
            }
            return base.VisitSwitchSection(node);
        }

        /// <summary>
        /// Visits the specified case pattern switch label syntax node.
        /// </summary>
        /// <param name="node">The case pattern switch label syntax node to visit.</param>
        /// <returns>The visited case pattern switch label syntax node, possibly modified.</returns>
        public override SyntaxNode VisitCasePatternSwitchLabel(CasePatternSwitchLabelSyntax node)
        {
            if (_visitCasePatternSwitchLabel != null)
            {
                var visit = _visitCasePatternSwitchLabel.Invoke(this, node);
                return (visit is CasePatternSwitchLabelSyntax newNode) ? base.VisitCasePatternSwitchLabel(newNode) : visit;
            }
            return base.VisitCasePatternSwitchLabel(node);
        }

        /// <summary>
        /// Visits the specified case switch label syntax node.
        /// </summary>
        /// <param name="node">The case switch label syntax node to visit.</param>
        /// <returns>The visited case switch label syntax node, possibly modified.</returns>
        public override SyntaxNode VisitCaseSwitchLabel(CaseSwitchLabelSyntax node)
        {
            if (_visitCaseSwitchLabel != null)
            {
                var visit = _visitCaseSwitchLabel.Invoke(this, node);
                return (visit is CaseSwitchLabelSyntax newNode) ? base.VisitCaseSwitchLabel(newNode) : visit;
            }
            return base.VisitCaseSwitchLabel(node);
        }

        /// <summary>
        /// Visits the specified default switch label syntax node.
        /// </summary>
        /// <param name="node">The default switch label syntax node to visit.</param>
        /// <returns>The visited default switch label syntax node, possibly modified.</returns>
        public override SyntaxNode VisitDefaultSwitchLabel(DefaultSwitchLabelSyntax node)
        {
            if (_visitDefaultSwitchLabel != null)
            {
                var visit = _visitDefaultSwitchLabel.Invoke(this, node);
                return (visit is DefaultSwitchLabelSyntax newNode) ? base.VisitDefaultSwitchLabel(newNode) : visit;
            }
            return base.VisitDefaultSwitchLabel(node);
        }

        /// <summary>
        /// Visits the specified switch expression syntax node.
        /// </summary>
        /// <param name="node">The switch expression syntax node to visit.</param>
        /// <returns>The visited switch expression syntax node, possibly modified.</returns>
        public override SyntaxNode VisitSwitchExpression(SwitchExpressionSyntax node)
        {
            if (_visitSwitchExpression != null)
            {
                var visit = _visitSwitchExpression.Invoke(this, node);
                return (visit is SwitchExpressionSyntax newNode) ? base.VisitSwitchExpression(newNode) : visit;
            }
            return base.VisitSwitchExpression(node);
        }

        /// <summary>
        /// Visits the specified switch expression arm syntax node.
        /// </summary>
        /// <param name="node">The switch expression arm syntax node to visit.</param>
        /// <returns>The visited switch expression arm syntax node, possibly modified.</returns>
        public override SyntaxNode VisitSwitchExpressionArm(SwitchExpressionArmSyntax node)
        {
            if (_visitSwitchExpressionArm != null)
            {
                var visit = _visitSwitchExpressionArm.Invoke(this, node);
                return (visit is SwitchExpressionArmSyntax newNode) ? base.VisitSwitchExpressionArm(newNode) : visit;
            }
            return base.VisitSwitchExpressionArm(node);
        }

        /// <summary>
        /// Visits the specified try statement syntax node.
        /// </summary>
        /// <param name="node">The try statement syntax node to visit.</param>
        /// <returns>The visited try statement syntax node, possibly modified.</returns>
        public override SyntaxNode VisitTryStatement(TryStatementSyntax node)
        {
            if (_visitTryStatement != null)
            {
                var visit = _visitTryStatement.Invoke(this, node);
                return (visit is TryStatementSyntax newNode) ? base.VisitTryStatement(newNode) : visit;
            }
            return base.VisitTryStatement(node);
        }

        /// <summary>
        /// Visits the specified catch clause syntax node.
        /// </summary>
        /// <param name="node">The catch clause syntax node to visit.</param>
        /// <returns>The visited catch clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitCatchClause(CatchClauseSyntax node)
        {
            if (_visitCatchClause != null)
            {
                var visit = _visitCatchClause.Invoke(this, node);
                return (visit is CatchClauseSyntax newNode) ? base.VisitCatchClause(newNode) : visit;
            }
            return base.VisitCatchClause(node);
        }

        /// <summary>
        /// Visits the specified catch declaration syntax node.
        /// </summary>
        /// <param name="node">The catch declaration syntax node to visit.</param>
        /// <returns>The visited catch declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitCatchDeclaration(CatchDeclarationSyntax node)
        {
            if (_visitCatchDeclaration != null)
            {
                var visit = _visitCatchDeclaration.Invoke(this, node);
                return (visit is CatchDeclarationSyntax newNode) ? base.VisitCatchDeclaration(newNode) : visit;
            }
            return base.VisitCatchDeclaration(node);
        }

        /// <summary>
        /// Visits the specified catch filter clause syntax node.
        /// </summary>
        /// <param name="node">The catch filter clause syntax node to visit.</param>
        /// <returns>The visited catch filter clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitCatchFilterClause(CatchFilterClauseSyntax node)
        {
            if (_visitCatchFilterClause != null)
            {
                var visit = _visitCatchFilterClause.Invoke(this, node);
                return (visit is CatchFilterClauseSyntax newNode) ? base.VisitCatchFilterClause(newNode) : visit;
            }
            return base.VisitCatchFilterClause(node);
        }

        /// <summary>
        /// Visits the specified finally clause syntax node.
        /// </summary>
        /// <param name="node">The finally clause syntax node to visit.</param>
        /// <returns>The visited finally clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitFinallyClause(FinallyClauseSyntax node)
        {
            if (_visitFinallyClause != null)
            {
                var visit = _visitFinallyClause.Invoke(this, node);
                return (visit is FinallyClauseSyntax newNode) ? base.VisitFinallyClause(newNode) : visit;
            }
            return base.VisitFinallyClause(node);
        }

        /// <summary>
        /// Visits the specified compilation unit syntax node.
        /// </summary>
        /// <param name="node">The compilation unit syntax node to visit.</param>
        /// <returns>The visited compilation unit syntax node, possibly modified.</returns>
        public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node)
        {
            if (_visitCompilationUnit != null)
            {
                var visit = _visitCompilationUnit.Invoke(this, node);
                return (visit is CompilationUnitSyntax newNode) ? base.VisitCompilationUnit(newNode) : visit;
            }
            return base.VisitCompilationUnit(node);
        }

        /// <summary>
        /// Visits the specified extern alias directive syntax node.
        /// </summary>
        /// <param name="node">The extern alias directive syntax node to visit.</param>
        /// <returns>The visited extern alias directive syntax node, possibly modified.</returns>
        public override SyntaxNode VisitExternAliasDirective(ExternAliasDirectiveSyntax node)
        {
            if (_visitExternAliasDirective != null)
            {
                var visit = _visitExternAliasDirective.Invoke(this, node);
                return (visit is ExternAliasDirectiveSyntax newNode) ? base.VisitExternAliasDirective(newNode) : visit;
            }
            return base.VisitExternAliasDirective(node);
        }

        /// <summary>
        /// Visits the specified using directive syntax node.
        /// </summary>
        /// <param name="node">The using directive syntax node to visit.</param>
        /// <returns>The visited using directive syntax node, possibly modified.</returns>
        public override SyntaxNode VisitUsingDirective(UsingDirectiveSyntax node)
        {
            if (_visitUsingDirective != null)
            {
                var visit = _visitUsingDirective.Invoke(this, node);
                return (visit is UsingDirectiveSyntax newNode) ? base.VisitUsingDirective(newNode) : visit;
            }
            return base.VisitUsingDirective(node);
        }

        /// <summary>
        /// Visits the specified namespace declaration syntax node.
        /// </summary>
        /// <param name="node">The namespace declaration syntax node to visit.</param>
        /// <returns>The visited namespace declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            if (_visitNamespaceDeclaration != null)
            {
                var visit = _visitNamespaceDeclaration.Invoke(this, node);
                return (visit is NamespaceDeclarationSyntax newNode) ? base.VisitNamespaceDeclaration(newNode) : visit;
            }
            return base.VisitNamespaceDeclaration(node);
        }

        /// <summary>
        /// Visits the specified file-scoped namespace declaration syntax node.
        /// </summary>
        /// <param name="node">The file-scoped namespace declaration syntax node to visit.</param>
        /// <returns>The visited file-scoped namespace declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node)
        {
            if (_visitFileScopedNamespaceDeclaration != null)
            {
                var visit = _visitFileScopedNamespaceDeclaration.Invoke(this, node);
                return (visit is FileScopedNamespaceDeclarationSyntax newNode) ? base.VisitFileScopedNamespaceDeclaration(newNode) : visit;
            }
            return base.VisitFileScopedNamespaceDeclaration(node);
        }

        /// <summary>
        /// Visits the specified attribute list syntax node.
        /// </summary>
        /// <param name="node">The attribute list syntax node to visit.</param>
        /// <returns>The visited attribute list syntax node, possibly modified.</returns>
        public override SyntaxNode VisitAttributeList(AttributeListSyntax node)
        {
            if (_visitAttributeList != null)
            {
                var visit = _visitAttributeList.Invoke(this, node);
                return (visit is AttributeListSyntax newNode) ? base.VisitAttributeList(newNode) : visit;
            }
            return base.VisitAttributeList(node);
        }

        /// <summary>
        /// Visits the specified attribute target specifier syntax node.
        /// </summary>
        /// <param name="node">The attribute target specifier syntax node to visit.</param>
        /// <returns>The visited attribute target specifier syntax node, possibly modified.</returns>
        public override SyntaxNode VisitAttributeTargetSpecifier(AttributeTargetSpecifierSyntax node)
        {
            if (_visitAttributeTargetSpecifier != null)
            {
                var visit = _visitAttributeTargetSpecifier.Invoke(this, node);
                return (visit is AttributeTargetSpecifierSyntax newNode) ? base.VisitAttributeTargetSpecifier(newNode) : visit;
            }
            return base.VisitAttributeTargetSpecifier(node);
        }

        /// <summary>
        /// Visits the specified attribute syntax node.
        /// </summary>
        /// <param name="node">The attribute syntax node to visit.</param>
        /// <returns>The visited attribute syntax node, possibly modified.</returns>
        public override SyntaxNode VisitAttribute(AttributeSyntax node)
        {
            if (_visitAttribute != null)
            {
                var visit = _visitAttribute.Invoke(this, node);
                return (visit is AttributeSyntax newNode) ? base.VisitAttribute(newNode) : visit;
            }
            return base.VisitAttribute(node);
        }

        /// <summary>
        /// Visits the specified attribute argument list syntax node.
        /// </summary>
        /// <param name="node">The attribute argument list syntax node to visit.</param>
        /// <returns>The visited attribute argument list syntax node, possibly modified.</returns>
        public override SyntaxNode VisitAttributeArgumentList(AttributeArgumentListSyntax node)
        {
            if (_visitAttributeArgumentList != null)
            {
                var visit = _visitAttributeArgumentList.Invoke(this, node);
                return (visit is AttributeArgumentListSyntax newNode) ? base.VisitAttributeArgumentList(newNode) : visit;
            }
            return base.VisitAttributeArgumentList(node);
        }

        /// <summary>
        /// Visits the specified attribute argument syntax node.
        /// </summary>
        /// <param name="node">The attribute argument syntax node to visit.</param>
        /// <returns>The visited attribute argument syntax node, possibly modified.</returns>
        public override SyntaxNode VisitAttributeArgument(AttributeArgumentSyntax node)
        {
            if (_visitAttributeArgument != null)
            {
                var visit = _visitAttributeArgument.Invoke(this, node);
                return (visit is AttributeArgumentSyntax newNode) ? base.VisitAttributeArgument(newNode) : visit;
            }
            return base.VisitAttributeArgument(node);
        }

        /// <summary>
        /// Visits the specified name equals syntax node.
        /// </summary>
        /// <param name="node">The name equals syntax node to visit.</param>
        /// <returns>The visited name equals syntax node, possibly modified.</returns>
        public override SyntaxNode VisitNameEquals(NameEqualsSyntax node)
        {
            if (_visitNameEquals != null)
            {
                var visit = _visitNameEquals.Invoke(this, node);
                return (visit is NameEqualsSyntax newNode) ? base.VisitNameEquals(newNode) : visit;
            }
            return base.VisitNameEquals(node);
        }

        /// <summary>
        /// Visits the specified type parameter list syntax node.
        /// </summary>
        /// <param name="node">The type parameter list syntax node to visit.</param>
        /// <returns>The visited type parameter list syntax node, possibly modified.</returns>
        public override SyntaxNode VisitTypeParameterList(TypeParameterListSyntax node)
        {
            if (_visitTypeParameterList != null)
            {
                var visit = _visitTypeParameterList.Invoke(this, node);
                return (visit is TypeParameterListSyntax newNode) ? base.VisitTypeParameterList(newNode) : visit;
            }
            return base.VisitTypeParameterList(node);
        }

        /// <summary>
        /// Visits the specified type parameter syntax node.
        /// </summary>
        /// <param name="node">The type parameter syntax node to visit.</param>
        /// <returns>The visited type parameter syntax node, possibly modified.</returns>
        public override SyntaxNode VisitTypeParameter(TypeParameterSyntax node)
        {
            if (_visitTypeParameter != null)
            {
                var visit = _visitTypeParameter.Invoke(this, node);
                return (visit is TypeParameterSyntax newNode) ? base.VisitTypeParameter(newNode) : visit;
            }
            return base.VisitTypeParameter(node);
        }

        /// <summary>
        /// Visits the specified class declaration syntax node.
        /// </summary>
        /// <param name="node">The class declaration syntax node to visit.</param>
        /// <returns>The visited class declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (_visitClassDeclaration != null)
            {
                var visit = _visitClassDeclaration.Invoke(this, node);
                return (visit is ClassDeclarationSyntax newNode) ? base.VisitClassDeclaration(newNode) : visit;
            }
            return base.VisitClassDeclaration(node);
        }

        /// <summary>
        /// Visits the specified struct declaration syntax node.
        /// </summary>
        /// <param name="node">The struct declaration syntax node to visit.</param>
        /// <returns>The visited struct declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node)
        {
            if (_visitStructDeclaration != null)
            {
                var visit = _visitStructDeclaration.Invoke(this, node);
                return (visit is StructDeclarationSyntax newNode) ? base.VisitStructDeclaration(newNode) : visit;
            }
            return base.VisitStructDeclaration(node);
        }

        /// <summary>
        /// Visits the specified interface declaration syntax node.
        /// </summary>
        /// <param name="node">The interface declaration syntax node to visit.</param>
        /// <returns>The visited interface declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            if (_visitInterfaceDeclaration != null)
            {
                var visit = _visitInterfaceDeclaration.Invoke(this, node);
                return (visit is InterfaceDeclarationSyntax newNode) ? base.VisitInterfaceDeclaration(newNode) : visit;
            }
            return base.VisitInterfaceDeclaration(node);
        }

        /// <summary>
        /// Visits the specified record declaration syntax node.
        /// </summary>
        /// <param name="node">The record declaration syntax node to visit.</param>
        /// <returns>The visited record declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            if (_visitRecordDeclaration != null)
            {
                var visit = _visitRecordDeclaration.Invoke(this, node);
                return (visit is RecordDeclarationSyntax newNode) ? base.VisitRecordDeclaration(newNode) : visit;
            }
            return base.VisitRecordDeclaration(node);
        }

        /// <summary>
        /// Visits the specified enum declaration syntax node.
        /// </summary>
        /// <param name="node">The enum declaration syntax node to visit.</param>
        /// <returns>The visited enum declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            if (_visitEnumDeclaration != null)
            {
                var visit = _visitEnumDeclaration.Invoke(this, node);
                return (visit is EnumDeclarationSyntax newNode) ? base.VisitEnumDeclaration(newNode) : visit;
            }
            return base.VisitEnumDeclaration(node);
        }

        /// <summary>
        /// Visits the specified delegate declaration syntax node.
        /// </summary>
        /// <param name="node">The delegate declaration syntax node to visit.</param>
        /// <returns>The visited delegate declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            if (_visitDelegateDeclaration != null)
            {
                var visit = _visitDelegateDeclaration.Invoke(this, node);
                return (visit is DelegateDeclarationSyntax newNode) ? base.VisitDelegateDeclaration(newNode) : visit;
            }
            return base.VisitDelegateDeclaration(node);
        }

        /// <summary>
        /// Visits the specified enum member declaration syntax node.
        /// </summary>
        /// <param name="node">The enum member declaration syntax node to visit.</param>
        /// <returns>The visited enum member declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
        {
            if (_visitEnumMemberDeclaration != null)
            {
                var visit = _visitEnumMemberDeclaration.Invoke(this, node);
                return (visit is EnumMemberDeclarationSyntax newNode) ? base.VisitEnumMemberDeclaration(newNode) : visit;
            }
            return base.VisitEnumMemberDeclaration(node);
        }

        /// <summary>
        /// Visits the specified base list syntax node.
        /// </summary>
        /// <param name="node">The base list syntax node to visit.</param>
        /// <returns>The visited base list syntax node, possibly modified.</returns>
        public override SyntaxNode VisitBaseList(BaseListSyntax node)
        {
            if (_visitBaseList != null)
            {
                var visit = _visitBaseList.Invoke(this, node);
                return (visit is BaseListSyntax newNode) ? base.VisitBaseList(newNode) : visit;
            }
            return base.VisitBaseList(node);
        }

        /// <summary>
        /// Visits the specified simple base type syntax node.
        /// </summary>
        /// <param name="node">The simple base type syntax node to visit.</param>
        /// <returns>The visited simple base type syntax node, possibly modified.</returns>
        public override SyntaxNode VisitSimpleBaseType(SimpleBaseTypeSyntax node)
        {
            if (_visitSimpleBaseType != null)
            {
                var visit = _visitSimpleBaseType.Invoke(this, node);
                return (visit is SimpleBaseTypeSyntax newNode) ? base.VisitSimpleBaseType(newNode) : visit;
            }
            return base.VisitSimpleBaseType(node);
        }

        /// <summary>
        /// Visits the specified primary constructor base type syntax node.
        /// </summary>
        /// <param name="node">The primary constructor base type syntax node to visit.</param>
        /// <returns>The visited primary constructor base type syntax node, possibly modified.</returns>
        public override SyntaxNode VisitPrimaryConstructorBaseType(PrimaryConstructorBaseTypeSyntax node)
        {
            if (_visitPrimaryConstructorBaseType != null)
            {
                var visit = _visitPrimaryConstructorBaseType.Invoke(this, node);
                return (visit is PrimaryConstructorBaseTypeSyntax newNode) ? base.VisitPrimaryConstructorBaseType(newNode) : visit;
            }
            return base.VisitPrimaryConstructorBaseType(node);
        }

        /// <summary>
        /// Visits the specified type parameter constraint clause syntax node.
        /// </summary>
        /// <param name="node">The type parameter constraint clause syntax node to visit.</param>
        /// <returns>The visited type parameter constraint clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node)
        {
            if (_visitTypeParameterConstraintClause != null)
            {
                var visit = _visitTypeParameterConstraintClause.Invoke(this, node);
                return (visit is TypeParameterConstraintClauseSyntax newNode) ? base.VisitTypeParameterConstraintClause(newNode) : visit;
            }
            return base.VisitTypeParameterConstraintClause(node);
        }

        /// <summary>
        /// Visits the specified constructor constraint syntax node.
        /// </summary>
        /// <param name="node">The constructor constraint syntax node to visit.</param>
        /// <returns>The visited constructor constraint syntax node, possibly modified.</returns>
        public override SyntaxNode VisitConstructorConstraint(ConstructorConstraintSyntax node)
        {
            if (_visitConstructorConstraint != null)
            {
                var visit = _visitConstructorConstraint.Invoke(this, node);
                return (visit is ConstructorConstraintSyntax newNode) ? base.VisitConstructorConstraint(newNode) : visit;
            }
            return base.VisitConstructorConstraint(node);
        }

        /// <summary>
        /// Visits the specified class or struct constraint syntax node.
        /// </summary>
        /// <param name="node">The class or struct constraint syntax node to visit.</param>
        /// <returns>The visited class or struct constraint syntax node, possibly modified.</returns>
        public override SyntaxNode VisitClassOrStructConstraint(ClassOrStructConstraintSyntax node)
        {
            if (_visitClassOrStructConstraint != null)
            {
                var visit = _visitClassOrStructConstraint.Invoke(this, node);
                return (visit is ClassOrStructConstraintSyntax newNode) ? base.VisitClassOrStructConstraint(newNode) : visit;
            }
            return base.VisitClassOrStructConstraint(node);
        }

        /// <summary>
        /// Visits the specified type constraint syntax node.
        /// </summary>
        /// <param name="node">The type constraint syntax node to visit.</param>
        /// <returns>The visited type constraint syntax node, possibly modified.</returns>
        public override SyntaxNode VisitTypeConstraint(TypeConstraintSyntax node)
        {
            if (_visitTypeConstraint != null)
            {
                var visit = _visitTypeConstraint.Invoke(this, node);
                return (visit is TypeConstraintSyntax newNode) ? base.VisitTypeConstraint(newNode) : visit;
            }
            return base.VisitTypeConstraint(node);
        }

        /// <summary>
        /// Visits the specified default constraint syntax node.
        /// </summary>
        /// <param name="node">The default constraint syntax node to visit.</param>
        /// <returns>The visited default constraint syntax node, possibly modified.</returns>
        public override SyntaxNode VisitDefaultConstraint(DefaultConstraintSyntax node)
        {
            if (_visitDefaultConstraint != null)
            {
                var visit = _visitDefaultConstraint.Invoke(this, node);
                return (visit is DefaultConstraintSyntax newNode) ? base.VisitDefaultConstraint(newNode) : visit;
            }
            return base.VisitDefaultConstraint(node);
        }

        /// <summary>
        /// Visits the specified field declaration syntax node.
        /// </summary>
        /// <param name="node">The field declaration syntax node to visit.</param>
        /// <returns>The visited field declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            if (_visitFieldDeclaration != null)
            {
                var visit = _visitFieldDeclaration.Invoke(this, node);
                return (visit is FieldDeclarationSyntax newNode) ? base.VisitFieldDeclaration(newNode) : visit;
            }
            return base.VisitFieldDeclaration(node);
        }

        /// <summary>
        /// Visits the specified event field declaration syntax node.
        /// </summary>
        /// <param name="node">The event field declaration syntax node to visit.</param>
        /// <returns>The visited event field declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            if (_visitEventFieldDeclaration != null)
            {
                var visit = _visitEventFieldDeclaration.Invoke(this, node);
                return (visit is EventFieldDeclarationSyntax newNode) ? base.VisitEventFieldDeclaration(newNode) : visit;
            }
            return base.VisitEventFieldDeclaration(node);
        }

        /// <summary>
        /// Visits the specified explicit interface specifier syntax node.
        /// </summary>
        /// <param name="node">The explicit interface specifier syntax node to visit.</param>
        /// <returns>The visited explicit interface specifier syntax node, possibly modified.</returns>
        public override SyntaxNode VisitExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax node)
        {
            if (_visitExplicitInterfaceSpecifier != null)
            {
                var visit = _visitExplicitInterfaceSpecifier.Invoke(this, node);
                return (visit is ExplicitInterfaceSpecifierSyntax newNode) ? base.VisitExplicitInterfaceSpecifier(newNode) : visit;
            }
            return base.VisitExplicitInterfaceSpecifier(node);
        }

        /// <summary>
        /// Visits the specified method declaration syntax node.
        /// </summary>
        /// <param name="node">The method declaration syntax node to visit.</param>
        /// <returns>The visited method declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (_visitMethodDeclaration != null)
            {
                var visit = _visitMethodDeclaration.Invoke(this, node);
                return (visit is MethodDeclarationSyntax newNode) ? base.VisitMethodDeclaration(newNode) : visit;
            }
            return base.VisitMethodDeclaration(node);
        }

        /// <summary>
        /// Visits the specified operator declaration syntax node.
        /// </summary>
        /// <param name="node">The operator declaration syntax node to visit.</param>
        /// <returns>The visited operator declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node)
        {
            if (_visitOperatorDeclaration != null)
            {
                var visit = _visitOperatorDeclaration.Invoke(this, node);
                return (visit is OperatorDeclarationSyntax newNode) ? base.VisitOperatorDeclaration(newNode) : visit;
            }
            return base.VisitOperatorDeclaration(node);
        }

        /// <summary>
        /// Visits the specified conversion operator declaration syntax node.
        /// </summary>
        /// <param name="node">The conversion operator declaration syntax node to visit.</param>
        /// <returns>The visited conversion operator declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
        {
            if (_visitConversionOperatorDeclaration != null)
            {
                var visit = _visitConversionOperatorDeclaration.Invoke(this, node);
                return (visit is ConversionOperatorDeclarationSyntax newNode) ? base.VisitConversionOperatorDeclaration(newNode) : visit;
            }
            return base.VisitConversionOperatorDeclaration(node);
        }

        /// <summary>
        /// Visits the specified constructor declaration syntax node.
        /// </summary>
        /// <param name="node">The constructor declaration syntax node to visit.</param>
        /// <returns>The visited constructor declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            if (_visitConstructorDeclaration != null)
            {
                var visit = _visitConstructorDeclaration.Invoke(this, node);
                return (visit is ConstructorDeclarationSyntax newNode) ? base.VisitConstructorDeclaration(newNode) : visit;
            }
            return base.VisitConstructorDeclaration(node);
        }

        /// <summary>
        /// Visits the specified constructor initializer syntax node.
        /// </summary>
        /// <param name="node">The constructor initializer syntax node to visit.</param>
        /// <returns>The visited constructor initializer syntax node, possibly modified.</returns>
        public override SyntaxNode VisitConstructorInitializer(ConstructorInitializerSyntax node)
        {
            if (_visitConstructorInitializer != null)
            {
                var visit = _visitConstructorInitializer.Invoke(this, node);
                return (visit is ConstructorInitializerSyntax newNode) ? base.VisitConstructorInitializer(newNode) : visit;
            }
            return base.VisitConstructorInitializer(node);
        }

        /// <summary>
        /// Visits the specified destructor declaration syntax node.
        /// </summary>
        /// <param name="node">The destructor declaration syntax node to visit.</param>
        /// <returns>The visited destructor declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitDestructorDeclaration(DestructorDeclarationSyntax node)
        {
            if (_visitDestructorDeclaration != null)
            {
                var visit = _visitDestructorDeclaration.Invoke(this, node);
                return (visit is DestructorDeclarationSyntax newNode) ? base.VisitDestructorDeclaration(newNode) : visit;
            }
            return base.VisitDestructorDeclaration(node);
        }

        /// <summary>
        /// Visits the specified property declaration syntax node.
        /// </summary>
        /// <param name="node">The property declaration syntax node to visit.</param>
        /// <returns>The visited property declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            if (_visitPropertyDeclaration != null)
            {
                var visit = _visitPropertyDeclaration.Invoke(this, node);
                return (visit is PropertyDeclarationSyntax newNode) ? base.VisitPropertyDeclaration(newNode) : visit;
            }
            return base.VisitPropertyDeclaration(node);
        }

        /// <summary>
        /// Visits the specified arrow expression clause syntax node.
        /// </summary>
        /// <param name="node">The arrow expression clause syntax node to visit.</param>
        /// <returns>The visited arrow expression clause syntax node, possibly modified.</returns>
        public override SyntaxNode VisitArrowExpressionClause(ArrowExpressionClauseSyntax node)
        {
            if (_visitArrowExpressionClause != null)
            {
                var visit = _visitArrowExpressionClause.Invoke(this, node);
                return (visit is ArrowExpressionClauseSyntax newNode) ? base.VisitArrowExpressionClause(newNode) : visit;
            }
            return base.VisitArrowExpressionClause(node);
        }

        /// <summary>
        /// Visits the specified event declaration syntax node.
        /// </summary>
        /// <param name="node">The event declaration syntax node to visit.</param>
        /// <returns>The visited event declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitEventDeclaration(EventDeclarationSyntax node)
        {
            if (_visitEventDeclaration != null)
            {
                var visit = _visitEventDeclaration.Invoke(this, node);
                return (visit is EventDeclarationSyntax newNode) ? base.VisitEventDeclaration(newNode) : visit;
            }
            return base.VisitEventDeclaration(node);
        }

        /// <summary>
        /// Visits the specified indexer declaration syntax node.
        /// </summary>
        /// <param name="node">The indexer declaration syntax node to visit.</param>
        /// <returns>The visited indexer declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitIndexerDeclaration(IndexerDeclarationSyntax node)
        {
            if (_visitIndexerDeclaration != null)
            {
                var visit = _visitIndexerDeclaration.Invoke(this, node);
                return (visit is IndexerDeclarationSyntax newNode) ? base.VisitIndexerDeclaration(newNode) : visit;
            }
            return base.VisitIndexerDeclaration(node);
        }

        /// <summary>
        /// Visits the specified accessor list syntax node.
        /// </summary>
        /// <param name="node">The accessor list syntax node to visit.</param>
        /// <returns>The visited accessor list syntax node, possibly modified.</returns>
        public override SyntaxNode VisitAccessorList(AccessorListSyntax node)
        {
            if (_visitAccessorList != null)
            {
                var visit = _visitAccessorList.Invoke(this, node);
                return (visit is AccessorListSyntax newNode) ? base.VisitAccessorList(newNode) : visit;
            }
            return base.VisitAccessorList(node);
        }

        /// <summary>
        /// Visits the specified accessor declaration syntax node.
        /// </summary>
        /// <param name="node">The accessor declaration syntax node to visit.</param>
        /// <returns>The visited accessor declaration syntax node, possibly modified.</returns>
        public override SyntaxNode VisitAccessorDeclaration(AccessorDeclarationSyntax node)
        {
            if (_visitAccessorDeclaration != null)
            {
                var visit = _visitAccessorDeclaration.Invoke(this, node);
                return (visit is AccessorDeclarationSyntax newNode) ? base.VisitAccessorDeclaration(newNode) : visit;
            }
            return base.VisitAccessorDeclaration(node);
        }

        /// <summary>
        /// Visits the specified parameter list syntax node.
        /// </summary>
        /// <param name="node">The parameter list syntax node to visit.</param>
        /// <returns>The visited parameter list syntax node, possibly modified.</returns>
        public override SyntaxNode VisitParameterList(ParameterListSyntax node)
        {
            if (_visitParameterList != null)
            {
                var visit = _visitParameterList.Invoke(this, node);
                return (visit is ParameterListSyntax newNode) ? base.VisitParameterList(newNode) : visit;
            }
            return base.VisitParameterList(node);
        }

        /// <summary>
        /// Visits the specified bracketed parameter list syntax node.
        /// </summary>
        /// <param name="node">The bracketed parameter list syntax node to visit.</param>
        /// <returns>The visited bracketed parameter list syntax node, possibly modified.</returns>
        public override SyntaxNode VisitBracketedParameterList(BracketedParameterListSyntax node)
        {
            if (_visitBracketedParameterList != null)
            {
                var visit = _visitBracketedParameterList.Invoke(this, node);
                return (visit is BracketedParameterListSyntax newNode) ? base.VisitBracketedParameterList(newNode) : visit;
            }
            return base.VisitBracketedParameterList(node);
        }

        /// <summary>
        /// Visits the specified parameter syntax node.
        /// </summary>
        /// <param name="node">The parameter syntax node to visit.</param>
        /// <returns>The visited parameter syntax node, possibly modified.</returns>
        public override SyntaxNode VisitParameter(ParameterSyntax node)
        {
            if (_visitParameter != null)
            {
                var visit = _visitParameter.Invoke(this, node);
                return (visit is ParameterSyntax newNode) ? base.VisitParameter(newNode) : visit;
            }
            return base.VisitParameter(node);
        }

        /// <summary>
        /// Visits the specified function pointer parameter syntax node.
        /// </summary>
        /// <param name="node">The function pointer parameter syntax node to visit.</param>
        /// <returns>The visited function pointer parameter syntax node, possibly modified.</returns>
        public override SyntaxNode VisitFunctionPointerParameter(FunctionPointerParameterSyntax node)
        {
            if (_visitFunctionPointerParameter != null)
            {
                var visit = _visitFunctionPointerParameter.Invoke(this, node);
                return (visit is FunctionPointerParameterSyntax newNode) ? base.VisitFunctionPointerParameter(newNode) : visit;
            }
            return base.VisitFunctionPointerParameter(node);
        }

        /// <summary>
        /// Visits the specified incomplete member syntax node.
        /// </summary>
        /// <param name="node">The incomplete member syntax node to visit.</param>
        /// <returns>The visited incomplete member syntax node, possibly modified.</returns>
        public override SyntaxNode VisitIncompleteMember(IncompleteMemberSyntax node)
        {
            if (_visitIncompleteMember != null)
            {
                var visit = _visitIncompleteMember.Invoke(this, node);
                return (visit is IncompleteMemberSyntax newNode) ? base.VisitIncompleteMember(newNode) : visit;
            }
            return base.VisitIncompleteMember(node);
        }

        /// <summary>
        /// Visits the specified skipped tokens trivia syntax node.
        /// </summary>
        /// <param name="node">The skipped tokens trivia syntax node to visit.</param>
        /// <returns>The visited skipped tokens trivia syntax node, possibly modified.</returns>
        public override SyntaxNode VisitSkippedTokensTrivia(SkippedTokensTriviaSyntax node)
        {
            if (_visitSkippedTokensTrivia != null)
            {
                var visit = _visitSkippedTokensTrivia.Invoke(this, node);
                return (visit is SkippedTokensTriviaSyntax newNode) ? base.VisitSkippedTokensTrivia(newNode) : visit;
            }
            return base.VisitSkippedTokensTrivia(node);
        }

        /// <summary>
        /// Visits the specified documentation comment trivia syntax node.
        /// </summary>
        /// <param name="node">The documentation comment trivia syntax node to visit.</param>
        /// <returns>The visited documentation comment trivia syntax node, possibly modified.</returns>
        public override SyntaxNode VisitDocumentationCommentTrivia(DocumentationCommentTriviaSyntax node)
        {
            if (_visitDocumentationCommentTrivia != null)
            {
                var visit = _visitDocumentationCommentTrivia.Invoke(this, node);
                return (visit is DocumentationCommentTriviaSyntax newNode) ? base.VisitDocumentationCommentTrivia(newNode) : visit;
            }
            return base.VisitDocumentationCommentTrivia(node);
        }

        /// <summary>
        /// Visits the specified type cref syntax node.
        /// </summary>
        /// <param name="node">The type cref syntax node to visit.</param>
        /// <returns>The visited type cref syntax node, possibly modified.</returns>
        public override SyntaxNode VisitTypeCref(TypeCrefSyntax node)
        {
            if (_visitTypeCref != null)
            {
                var visit = _visitTypeCref.Invoke(this, node);
                return (visit is TypeCrefSyntax newNode) ? base.VisitTypeCref(newNode) : visit;
            }
            return base.VisitTypeCref(node);
        }

        /// <summary>
        /// Visits the specified qualified cref syntax node.
        /// </summary>
        /// <param name="node">The qualified cref syntax node to visit.</param>
        /// <returns>The visited qualified cref syntax node, possibly modified.</returns>
        public override SyntaxNode VisitQualifiedCref(QualifiedCrefSyntax node)
        {
            if (_visitQualifiedCref != null)
            {
                var visit = _visitQualifiedCref.Invoke(this, node);
                return (visit is QualifiedCrefSyntax newNode) ? base.VisitQualifiedCref(newNode) : visit;
            }
            return base.VisitQualifiedCref(node);
        }

        /// <summary>
        /// Visits the specified name member cref syntax node.
        /// </summary>
        /// <param name="node">The name member cref syntax node to visit.</param>
        /// <returns>The visited name member cref syntax node, possibly modified.</returns>
        public override SyntaxNode VisitNameMemberCref(NameMemberCrefSyntax node)
        {
            if (_visitNameMemberCref != null)
            {
                var visit = _visitNameMemberCref.Invoke(this, node);
                return (visit is NameMemberCrefSyntax newNode) ? base.VisitNameMemberCref(newNode) : visit;
            }
            return base.VisitNameMemberCref(node);
        }

        /// <summary>
        /// Visits the specified indexer member cref syntax node.
        /// </summary>
        /// <param name="node">The indexer member cref syntax node to visit.</param>
        /// <returns>The visited indexer member cref syntax node, possibly modified.</returns>
        public override SyntaxNode VisitIndexerMemberCref(IndexerMemberCrefSyntax node)
        {
            if (_visitIndexerMemberCref != null)
            {
                var visit = _visitIndexerMemberCref.Invoke(this, node);
                return (visit is IndexerMemberCrefSyntax newNode) ? base.VisitIndexerMemberCref(newNode) : visit;
            }
            return base.VisitIndexerMemberCref(node);
        }

        /// <summary>
        /// Visits the specified operator member cref syntax node.
        /// </summary>
        /// <param name="node">The operator member cref syntax node to visit.</param>
        /// <returns>The visited operator member cref syntax node, possibly modified.</returns>
        public override SyntaxNode VisitOperatorMemberCref(OperatorMemberCrefSyntax node)
        {
            if (_visitOperatorMemberCref != null)
            {
                var visit = _visitOperatorMemberCref.Invoke(this, node);
                return (visit is OperatorMemberCrefSyntax newNode) ? base.VisitOperatorMemberCref(newNode) : visit;
            }
            return base.VisitOperatorMemberCref(node);
        }

        /// <summary>
        /// Visits the specified conversion operator member cref syntax node.
        /// </summary>
        /// <param name="node">The conversion operator member cref syntax node to visit.</param>
        /// <returns>The visited conversion operator member cref syntax node, possibly modified.</returns>
        public override SyntaxNode VisitConversionOperatorMemberCref(ConversionOperatorMemberCrefSyntax node)
        {
            if (_visitConversionOperatorMemberCref != null)
            {
                var visit = _visitConversionOperatorMemberCref.Invoke(this, node);
                return (visit is ConversionOperatorMemberCrefSyntax newNode) ? base.VisitConversionOperatorMemberCref(newNode) : visit;
            }
            return base.VisitConversionOperatorMemberCref(node);
        }

        /// <summary>
        /// Visits the specified cref parameter list syntax node.
        /// </summary>
        /// <param name="node">The cref parameter list syntax node to visit.</param>
        /// <returns>The visited cref parameter list syntax node, possibly modified.</returns>
        public override SyntaxNode VisitCrefParameterList(CrefParameterListSyntax node)
        {
            if (_visitCrefParameterList != null)
            {
                var visit = _visitCrefParameterList.Invoke(this, node);
                return (visit is CrefParameterListSyntax newNode) ? base.VisitCrefParameterList(newNode) : visit;
            }
            return base.VisitCrefParameterList(node);
        }

        /// <summary>
        /// Visits the specified cref bracketed parameter list syntax node.
        /// </summary>
        /// <param name="node">The cref bracketed parameter list syntax node to visit.</param>
        /// <returns>The visited cref bracketed parameter list syntax node, possibly modified.</returns>
        public override SyntaxNode VisitCrefBracketedParameterList(CrefBracketedParameterListSyntax node)
        {
            if (_visitCrefBracketedParameterList != null)
            {
                var visit = _visitCrefBracketedParameterList.Invoke(this, node);
                return (visit is CrefBracketedParameterListSyntax newNode) ? base.VisitCrefBracketedParameterList(newNode) : visit;
            }
            return base.VisitCrefBracketedParameterList(node);
        }

        /// <summary>
        /// Visits the specified cref parameter syntax node.
        /// </summary>
        /// <param name="node">The cref parameter syntax node to visit.</param>
        /// <returns>The visited cref parameter syntax node, possibly modified.</returns>
        public override SyntaxNode VisitCrefParameter(CrefParameterSyntax node)
        {
            if (_visitCrefParameter != null)
            {
                var visit = _visitCrefParameter.Invoke(this, node);
                return (visit is CrefParameterSyntax newNode) ? base.VisitCrefParameter(newNode) : visit;
            }
            return base.VisitCrefParameter(node);
        }

        /// <summary>
        /// Visits the specified XML element syntax node.
        /// </summary>
        /// <param name="node">The XML element syntax node to visit.</param>
        /// <returns>The visited XML element syntax node, possibly modified.</returns>
        public override SyntaxNode VisitXmlElement(XmlElementSyntax node)
        {
            if (_visitXmlElement != null)
            {
                var visit = _visitXmlElement.Invoke(this, node);
                return (visit is XmlElementSyntax newNode) ? base.VisitXmlElement(newNode) : visit;
            }
            return base.VisitXmlElement(node);
        }

        /// <summary>
        /// Visits the specified XML element start tag syntax node.
        /// </summary>
        /// <param name="node">The XML element start tag syntax node to visit.</param>
        /// <returns>The visited XML element start tag syntax node, possibly modified.</returns>
        public override SyntaxNode VisitXmlElementStartTag(XmlElementStartTagSyntax node)
        {
            if (_visitXmlElementStartTag != null)
            {
                var visit = _visitXmlElementStartTag.Invoke(this, node);
                return (visit is XmlElementStartTagSyntax newNode) ? base.VisitXmlElementStartTag(newNode) : visit;
            }
            return base.VisitXmlElementStartTag(node);
        }

        /// <summary>
        /// Visits the specified XML element end tag syntax node.
        /// </summary>
        /// <param name="node">The XML element end tag syntax node to visit.</param>
        /// <returns>The visited XML element end tag syntax node, possibly modified.</returns>
        public override SyntaxNode VisitXmlElementEndTag(XmlElementEndTagSyntax node)
        {
            if (_visitXmlElementEndTag != null)
            {
                var visit = _visitXmlElementEndTag.Invoke(this, node);
                return (visit is XmlElementEndTagSyntax newNode) ? base.VisitXmlElementEndTag(newNode) : visit;
            }
            return base.VisitXmlElementEndTag(node);
        }

        /// <summary>
        /// Visits the specified XML empty element syntax node.
        /// </summary>
        /// <param name="node">The XML empty element syntax node to visit.</param>
        /// <returns>The visited XML empty element syntax node, possibly modified.</returns>
        public override SyntaxNode VisitXmlEmptyElement(XmlEmptyElementSyntax node)
        {
            if (_visitXmlEmptyElement != null)
            {
                var visit = _visitXmlEmptyElement.Invoke(this, node);
                return (visit is XmlEmptyElementSyntax newNode) ? base.VisitXmlEmptyElement(newNode) : visit;
            }
            return base.VisitXmlEmptyElement(node);
        }

        /// <summary>
        /// Visits the specified XML name syntax node.
        /// </summary>
        /// <param name="node">The XML name syntax node to visit.</param>
        /// <returns>The visited XML name syntax node, possibly modified.</returns>
        public override SyntaxNode VisitXmlName(XmlNameSyntax node)
        {
            if (_visitXmlName != null)
            {
                var visit = _visitXmlName.Invoke(this, node);
                return (visit is XmlNameSyntax newNode) ? base.VisitXmlName(newNode) : visit;
            }
            return base.VisitXmlName(node);
        }

        /// <summary>
        /// Visits the specified XML prefix syntax node.
        /// </summary>
        /// <param name="node">The XML prefix syntax node to visit.</param>
        /// <returns>The visited XML prefix syntax node, possibly modified.</returns>
        public override SyntaxNode VisitXmlPrefix(XmlPrefixSyntax node)
        {
            if (_visitXmlPrefix != null)
            {
                var visit = _visitXmlPrefix.Invoke(this, node);
                return (visit is XmlPrefixSyntax newNode) ? base.VisitXmlPrefix(newNode) : visit;
            }
            return base.VisitXmlPrefix(node);
        }

        /// <summary>
        /// Visits the specified XML text attribute syntax node.
        /// </summary>
        /// <param name="node">The XML text attribute syntax node to visit.</param>
        /// <returns>The visited XML text attribute syntax node, possibly modified.</returns>
        public override SyntaxNode VisitXmlTextAttribute(XmlTextAttributeSyntax node)
        {
            if (_visitXmlTextAttribute != null)
            {
                var visit = _visitXmlTextAttribute.Invoke(this, node);
                return (visit is XmlTextAttributeSyntax newNode) ? base.VisitXmlTextAttribute(newNode) : visit;
            }
            return base.VisitXmlTextAttribute(node);
        }

/// <summary>
/// Visits the specified XML cref attribute syntax node.
/// </summary>
/// <param name="node">The XML cref attribute syntax node to visit.</param>
/// <returns>The visited XML cref attribute syntax node, possibly modified.</returns>
public override SyntaxNode VisitXmlCrefAttribute(XmlCrefAttributeSyntax node)
{
    if (_visitXmlCrefAttribute != null)
    {
        var visit = _visitXmlCrefAttribute.Invoke(this, node);
        return (visit is XmlCrefAttributeSyntax newNode) ? base.VisitXmlCrefAttribute(newNode) : visit;
    }
    return base.VisitXmlCrefAttribute(node);
}

/// <summary>
/// Visits the specified XML name attribute syntax node.
/// </summary>
/// <param name="node">The XML name attribute syntax node to visit.</param>
/// <returns>The visited XML name attribute syntax node, possibly modified.</returns>
public override SyntaxNode VisitXmlNameAttribute(XmlNameAttributeSyntax node)
{
    if (_visitXmlNameAttribute != null)
    {
        var visit = _visitXmlNameAttribute.Invoke(this, node);
        return (visit is XmlNameAttributeSyntax newNode) ? base.VisitXmlNameAttribute(newNode) : visit;
    }
    return base.VisitXmlNameAttribute(node);
}

/// <summary>
/// Visits the specified XML text syntax node.
/// </summary>
/// <param name="node">The XML text syntax node to visit.</param>
/// <returns>The visited XML text syntax node, possibly modified.</returns>
public override SyntaxNode VisitXmlText(XmlTextSyntax node)
{
    if (_visitXmlText != null)
    {
        var visit = _visitXmlText.Invoke(this, node);
        return (visit is XmlTextSyntax newNode) ? base.VisitXmlText(newNode) : visit;
    }
    return base.VisitXmlText(node);
}

/// <summary>
/// Visits the specified XML CDATA section syntax node.
/// </summary>
/// <param name="node">The XML CDATA section syntax node to visit.</param>
/// <returns>The visited XML CDATA section syntax node, possibly modified.</returns>
public override SyntaxNode VisitXmlCDataSection(XmlCDataSectionSyntax node)
{
    if (_visitXmlCDataSection != null)
    {
        var visit = _visitXmlCDataSection.Invoke(this, node);
        return (visit is XmlCDataSectionSyntax newNode) ? base.VisitXmlCDataSection(newNode) : visit;
    }
    return base.VisitXmlCDataSection(node);
}

/// <summary>
/// Visits the specified XML processing instruction syntax node.
/// </summary>
/// <param name="node">The XML processing instruction syntax node to visit.</param>
/// <returns>The visited XML processing instruction syntax node, possibly modified.</returns>
public override SyntaxNode VisitXmlProcessingInstruction(XmlProcessingInstructionSyntax node)
{
    if (_visitXmlProcessingInstruction != null)
    {
        var visit = _visitXmlProcessingInstruction.Invoke(this, node);
        return (visit is XmlProcessingInstructionSyntax newNode) ? base.VisitXmlProcessingInstruction(newNode) : visit;
    }
    return base.VisitXmlProcessingInstruction(node);
}

/// <summary>
/// Visits the specified XML comment syntax node.
/// </summary>
/// <param name="node">The XML comment syntax node to visit.</param>
/// <returns>The visited XML comment syntax node, possibly modified.</returns>
public override SyntaxNode VisitXmlComment(XmlCommentSyntax node)
{
    if (_visitXmlComment != null)
    {
        var visit = _visitXmlComment.Invoke(this, node);
        return (visit is XmlCommentSyntax newNode) ? base.VisitXmlComment(newNode) : visit;
    }
    return base.VisitXmlComment(node);
}

/// <summary>
/// Visits the specified if directive trivia syntax node.
/// </summary>
/// <param name="node">The if directive trivia syntax node to visit.</param>
/// <returns>The visited if directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitIfDirectiveTrivia(IfDirectiveTriviaSyntax node)
{
    if (_visitIfDirectiveTrivia != null)
    {
        var visit = _visitIfDirectiveTrivia.Invoke(this, node);
        return (visit is IfDirectiveTriviaSyntax newNode) ? base.VisitIfDirectiveTrivia(newNode) : visit;
    }
    return base.VisitIfDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified elif directive trivia syntax node.
/// </summary>
/// <param name="node">The elif directive trivia syntax node to visit.</param>
/// <returns>The visited elif directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitElifDirectiveTrivia(ElifDirectiveTriviaSyntax node)
{
    if (_visitElifDirectiveTrivia != null)
    {
        var visit = _visitElifDirectiveTrivia.Invoke(this, node);
        return (visit is ElifDirectiveTriviaSyntax newNode) ? base.VisitElifDirectiveTrivia(newNode) : visit;
    }
    return base.VisitElifDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified else directive trivia syntax node.
/// </summary>
/// <param name="node">The else directive trivia syntax node to visit.</param>
/// <returns>The visited else directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitElseDirectiveTrivia(ElseDirectiveTriviaSyntax node)
{
    if (_visitElseDirectiveTrivia != null)
    {
        var visit = _visitElseDirectiveTrivia.Invoke(this, node);
        return (visit is ElseDirectiveTriviaSyntax newNode) ? base.VisitElseDirectiveTrivia(newNode) : visit;
    }
    return base.VisitElseDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified endif directive trivia syntax node.
/// </summary>
/// <param name="node">The endif directive trivia syntax node to visit.</param>
/// <returns>The visited endif directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitEndIfDirectiveTrivia(EndIfDirectiveTriviaSyntax node)
{
    if (_visitEndIfDirectiveTrivia != null)
    {
        var visit = _visitEndIfDirectiveTrivia.Invoke(this, node);
        return (visit is EndIfDirectiveTriviaSyntax newNode) ? base.VisitEndIfDirectiveTrivia(newNode) : visit;
    }
    return base.VisitEndIfDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified region directive trivia syntax node.
/// </summary>
/// <param name="node">The region directive trivia syntax node to visit.</param>
/// <returns>The visited region directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node)
{
    if (_visitRegionDirectiveTrivia != null)
    {
        var visit = _visitRegionDirectiveTrivia.Invoke(this, node);
        return (visit is RegionDirectiveTriviaSyntax newNode) ? base.VisitRegionDirectiveTrivia(newNode) : visit;
    }
    return base.VisitRegionDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified endregion directive trivia syntax node.
/// </summary>
/// <param name="node">The endregion directive trivia syntax node to visit.</param>
/// <returns>The visited endregion directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node)
{
    if (_visitEndRegionDirectiveTrivia != null)
    {
        var visit = _visitEndRegionDirectiveTrivia.Invoke(this, node);
        return (visit is EndRegionDirectiveTriviaSyntax newNode) ? base.VisitEndRegionDirectiveTrivia(newNode) : visit;
    }
    return base.VisitEndRegionDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified error directive trivia syntax node.
/// </summary>
/// <param name="node">The error directive trivia syntax node to visit.</param>
/// <returns>The visited error directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitErrorDirectiveTrivia(ErrorDirectiveTriviaSyntax node)
{
    if (_visitErrorDirectiveTrivia != null)
    {
        var visit = _visitErrorDirectiveTrivia.Invoke(this, node);
        return (visit is ErrorDirectiveTriviaSyntax newNode) ? base.VisitErrorDirectiveTrivia(newNode) : visit;
    }
    return base.VisitErrorDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified warning directive trivia syntax node.
/// </summary>
/// <param name="node">The warning directive trivia syntax node to visit.</param>
/// <returns>The visited warning directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitWarningDirectiveTrivia(WarningDirectiveTriviaSyntax node)
{
    if (_visitWarningDirectiveTrivia != null)
    {
        var visit = _visitWarningDirectiveTrivia.Invoke(this, node);
        return (visit is WarningDirectiveTriviaSyntax newNode) ? base.VisitWarningDirectiveTrivia(newNode) : visit;
    }
    return base.VisitWarningDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified bad directive trivia syntax node.
/// </summary>
/// <param name="node">The bad directive trivia syntax node to visit.</param>
/// <returns>The visited bad directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitBadDirectiveTrivia(BadDirectiveTriviaSyntax node)
{
    if (_visitBadDirectiveTrivia != null)
    {
        var visit = _visitBadDirectiveTrivia.Invoke(this, node);
        return (visit is BadDirectiveTriviaSyntax newNode) ? base.VisitBadDirectiveTrivia(newNode) : visit;
    }
    return base.VisitBadDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified define directive trivia syntax node.
/// </summary>
/// <param name="node">The define directive trivia syntax node to visit.</param>
/// <returns>The visited define directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitDefineDirectiveTrivia(DefineDirectiveTriviaSyntax node)
{
    if (_visitDefineDirectiveTrivia != null)
    {
        var visit = _visitDefineDirectiveTrivia.Invoke(this, node);
        return (visit is DefineDirectiveTriviaSyntax newNode) ? base.VisitDefineDirectiveTrivia(newNode) : visit;
    }
    return base.VisitDefineDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified undef directive trivia syntax node.
/// </summary>
/// <param name="node">The undef directive trivia syntax node to visit.</param>
/// <returns>The visited undef directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitUndefDirectiveTrivia(UndefDirectiveTriviaSyntax node)
{
    if (_visitUndefDirectiveTrivia != null)
    {
        var visit = _visitUndefDirectiveTrivia.Invoke(this, node);
        return (visit is UndefDirectiveTriviaSyntax newNode) ? base.VisitUndefDirectiveTrivia(newNode) : visit;
    }
    return base.VisitUndefDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified line directive trivia syntax node.
/// </summary>
/// <param name="node">The line directive trivia syntax node to visit.</param>
/// <returns>The visited line directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitLineDirectiveTrivia(LineDirectiveTriviaSyntax node)
{
    if (_visitLineDirectiveTrivia != null)
    {
        var visit = _visitLineDirectiveTrivia.Invoke(this, node);
        return (visit is LineDirectiveTriviaSyntax newNode) ? base.VisitLineDirectiveTrivia(newNode) : visit;
    }
    return base.VisitLineDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified line directive position syntax node.
/// </summary>
/// <param name="node">The line directive position syntax node to visit.</param>
/// <returns>The visited line directive position syntax node, possibly modified.</returns>
public override SyntaxNode VisitLineDirectivePosition(LineDirectivePositionSyntax node)
{
    if (_visitLineDirectivePosition != null)
    {
        var visit = _visitLineDirectivePosition.Invoke(this, node);
        return (visit is LineDirectivePositionSyntax newNode) ? base.VisitLineDirectivePosition(newNode) : visit;
    }
    return base.VisitLineDirectivePosition(node);
}

/// <summary>
/// Visits the specified line span directive trivia syntax node.
/// </summary>
/// <param name="node">The line span directive trivia syntax node to visit.</param>
/// <returns>The visited line span directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitLineSpanDirectiveTrivia(LineSpanDirectiveTriviaSyntax node)
{
    if (_visitLineSpanDirectiveTrivia != null)
    {
        var visit = _visitLineSpanDirectiveTrivia.Invoke(this, node);
        return (visit is LineSpanDirectiveTriviaSyntax newNode) ? base.VisitLineSpanDirectiveTrivia(newNode) : visit;
    }
    return base.VisitLineSpanDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified pragma warning directive trivia syntax node.
/// </summary>
/// <param name="node">The pragma warning directive trivia syntax node to visit.</param>
/// <returns>The visited pragma warning directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitPragmaWarningDirectiveTrivia(PragmaWarningDirectiveTriviaSyntax node)
{
    if (_visitPragmaWarningDirectiveTrivia != null)
    {
        var visit = _visitPragmaWarningDirectiveTrivia.Invoke(this, node);
        return (visit is PragmaWarningDirectiveTriviaSyntax newNode) ? base.VisitPragmaWarningDirectiveTrivia(newNode) : visit;
    }
    return base.VisitPragmaWarningDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified pragma checksum directive trivia syntax node.
/// </summary>
/// <param name="node">The pragma checksum directive trivia syntax node to visit.</param>
/// <returns>The visited pragma checksum directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitPragmaChecksumDirectiveTrivia(PragmaChecksumDirectiveTriviaSyntax node)
{
    if (_visitPragmaChecksumDirectiveTrivia != null)
    {
        var visit = _visitPragmaChecksumDirectiveTrivia.Invoke(this, node);
        return (visit is PragmaChecksumDirectiveTriviaSyntax newNode) ? base.VisitPragmaChecksumDirectiveTrivia(newNode) : visit;
    }
    return base.VisitPragmaChecksumDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified reference directive trivia syntax node.
/// </summary>
/// <param name="node">The reference directive trivia syntax node to visit.</param>
/// <returns>The visited reference directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitReferenceDirectiveTrivia(ReferenceDirectiveTriviaSyntax node)
{
    if (_visitReferenceDirectiveTrivia != null)
    {
        var visit = _visitReferenceDirectiveTrivia.Invoke(this, node);
        return (visit is ReferenceDirectiveTriviaSyntax newNode) ? base.VisitReferenceDirectiveTrivia(newNode) : visit;
    }
    return base.VisitReferenceDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified load directive trivia syntax node.
/// </summary>
/// <param name="node">The load directive trivia syntax node to visit.</param>
/// <returns>The visited load directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitLoadDirectiveTrivia(LoadDirectiveTriviaSyntax node)
{
    if (_visitLoadDirectiveTrivia != null)
    {
        var visit = _visitLoadDirectiveTrivia.Invoke(this, node);
        return (visit is LoadDirectiveTriviaSyntax newNode) ? base.VisitLoadDirectiveTrivia(newNode) : visit;
    }
    return base.VisitLoadDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified shebang directive trivia syntax node.
/// </summary>
/// <param name="node">The shebang directive trivia syntax node to visit.</param>
/// <returns>The visited shebang directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitShebangDirectiveTrivia(ShebangDirectiveTriviaSyntax node)
{
    if (_visitShebangDirectiveTrivia != null)
    {
        var visit = _visitShebangDirectiveTrivia.Invoke(this, node);
        return (visit is ShebangDirectiveTriviaSyntax newNode) ? base.VisitShebangDirectiveTrivia(newNode) : visit;
    }
    return base.VisitShebangDirectiveTrivia(node);
}

/// <summary>
/// Visits the specified nullable directive trivia syntax node.
/// </summary>
/// <param name="node">The nullable directive trivia syntax node to visit.</param>
/// <returns>The visited nullable directive trivia syntax node, possibly modified.</returns>
public override SyntaxNode VisitNullableDirectiveTrivia(NullableDirectiveTriviaSyntax node)
{
    if (_visitNullableDirectiveTrivia != null)
    {
        var visit = _visitNullableDirectiveTrivia.Invoke(this, node);
        return (visit is NullableDirectiveTriviaSyntax newNode) ? base.VisitNullableDirectiveTrivia(newNode) : visit;
    }
    return base.VisitNullableDirectiveTrivia(node);
}

    }
}
